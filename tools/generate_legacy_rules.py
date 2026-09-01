from __future__ import annotations

import json
import re
import unicodedata
from dataclasses import dataclass
from decimal import Decimal, InvalidOperation
from pathlib import Path
from typing import Any

from openpyxl import load_workbook


ROOT = Path(__file__).resolve().parents[1]
CATALOG_PATH = ROOT / "Legacy" / "MaterialCatalog.json"
OUTPUT_PATH = ROOT / "src" / "ZMT.SteelCost.Application" / "Calculation" / "LegacyExcelV1Rules.g.cs"


NUMERIC_INPUTS = {
    "B2": "BuildingArea",
    "B3": "EstimatedSteelKgPerM2",
    "B4": "CornerCount",
    "B5": "GroundFloorWidth",
    "B6": "GroundFloorLength",
    "B7": "FloorCount",
    "B8": "IntermediateFloorArea",
    "B9": "FloorHeight",
    "B11": "RoofSlope",
    "B12": "RoofFootprintArea",
    "B16": "EaveWidth",
    "B17": "EaveLength",
    "B18": "GableLength",
    "B21": "RidgeQuantity",
    "B22": "ParapetCoverQuantity",
    "B23": "MetalTileRidgeQuantity",
    "B24": "NarrowRidgeQuantity",
    "B25": "WideRidgeQuantity",
    "B26": "MetalBoardQuantity",
    "B29": "WetAreaWallLength",
    "B30": "WetAreaCeilingArea",
    "B32": "ExteriorWallLength",
    "B33": "InteriorWallLength",
    "B34": "RoofCoverArea",
    "B35": "CeilingArea",
    "B36": "EaveArea",
    "B39": "SteelDoorQuantity",
    "B40": "PvcDoorQuantity",
    "B41": "DoublePvcDoorQuantity",
    "B42": "MelamineDoorQuantity",
    "B43": "AmericanDoorQuantity",
    "B46": "Window105X180Quantity",
    "B47": "Window59X180Quantity",
    "B48": "Window80X120Quantity",
    "B49": "Window140X100Quantity",
    "B50": "Window140X140Quantity",
    "B51": "Window140X160Quantity",
    "B52": "Window140X180Quantity",
    "B53": "Window160X120Quantity",
    "B54": "Window160X160Quantity",
    "B55": "Window160X180Quantity",
    "B56": "SlidingWindow180X200Quantity",
    "B57": "TransomWindow60X60Quantity",
    "B61": "GroundFloorToiletQuantity",
    "E61": "FirstFloorToiletQuantity",
    "B62": "GroundFloorWashbasinQuantity",
    "E62": "FirstFloorWashbasinQuantity",
    "B63": "GroundFloorSquatToiletQuantity",
    "E63": "FirstFloorSquatToiletQuantity",
    "B64": "GroundFloorShowerTrayQuantity",
    "E64": "FirstFloorShowerTrayQuantity",
    "C3": "ExteriorWallThicknessMm",
    "E3": "InteriorWallThicknessMm",
    "C15": "PurlinCount3000",
    "D15": "OmegaCount2500",
    "C61": "LegacyUnusedC61",
}

TEXT_INPUTS = {
    "B13": "RoofCoverType",
    "B14": "RoofSystem",
    "C46": "WindowColor",
    "D32": "ExteriorLayer1",
    "E32": "ExteriorLayer2",
    "F32": "ExteriorLayer3",
    "D33": "InteriorLayer1",
    "E33": "InteriorLayer2",
    "F33": "InteriorLayer3",
    "D34": "RoofLayer1",
    "E34": "RoofLayer2",
    "F34": "RoofLayer3",
    "D35": "CeilingLayer1",
    "E35": "CeilingLayer2",
    "F35": "CeilingLayer3",
}

PRICING_CELLS = {
    "G2": "ExchangeRate",
    "H2": "SteelPrice",
    "N2": "SSeriesPrice",
    "O2": "GalvanizedPrice",
    "P2": "PaintedSheetPrice",
}


def stable_code(value: str) -> str:
    table = str.maketrans({
        "ı": "i", "İ": "I", "ş": "s", "Ş": "S", "ğ": "g", "Ğ": "G",
        "ü": "u", "Ü": "U", "ö": "o", "Ö": "O", "ç": "c", "Ç": "C",
    })
    normalized = unicodedata.normalize("NFKD", value.translate(table))
    ascii_value = "".join(char for char in normalized if not unicodedata.combining(char))
    return re.sub(r"[^A-Za-z0-9]+", "_", ascii_value).strip("_").upper()


def cs_string(value: str | None) -> str:
    if value is None:
        return "null"
    return json.dumps(str(value), ensure_ascii=False)


def cs_decimal(value: Any) -> str:
    if value is None or value == "":
        return "0m"
    try:
        number = Decimal(str(value))
    except InvalidOperation:
        return "0m"
    text = format(number, "f")
    if "." in text:
        text = text.rstrip("0").rstrip(".") or "0"
    return f"{text}m"


@dataclass(frozen=True)
class Token:
    kind: str
    text: str


TOKEN_PATTERN = re.compile(
    r"(?P<SPACE>\s+)"
    r"|(?P<SHEETCELL>'(?:[^']|'')+'!\$?[A-Z]{1,3}\$?\d+)"
    r'|(?P<STRING>"(?:[^"]|"")*")'
    r"|(?P<NUMBER>\d+(?:\.\d+)?)"
    r"|(?P<CELL>\$?[A-Z]{1,3}\$?\d+)"
    r"|(?P<IDENT>[A-Z][A-Z0-9_.]*)"
    r"|(?P<OP><>|>=|<=|=|>|<|\+|-|\*|/|\^|\(|\)|,|:)"
)


class Node:
    def emit(self, context: "EmitContext") -> "Emitted":
        raise NotImplementedError


@dataclass(frozen=True)
class Emitted:
    code: str
    kind: str


@dataclass(frozen=True)
class NumberNode(Node):
    value: str

    def emit(self, context: "EmitContext") -> Emitted:
        return Emitted(cs_decimal(self.value), "number")


@dataclass(frozen=True)
class StringNode(Node):
    value: str

    def emit(self, context: "EmitContext") -> Emitted:
        return Emitted(cs_string(stable_code(self.value)), "string")


@dataclass(frozen=True)
class CellNode(Node):
    sheet: str | None
    coordinate: str

    def emit(self, context: "EmitContext") -> Emitted:
        return context.emit_cell(self)


@dataclass(frozen=True)
class RangeNode(Node):
    start: CellNode
    end: CellNode

    def emit(self, context: "EmitContext") -> Emitted:
        return context.emit_range(self)


@dataclass(frozen=True)
class UnaryNode(Node):
    operator: str
    operand: Node

    def emit(self, context: "EmitContext") -> Emitted:
        item = self.operand.emit(context)
        return Emitted(f"({self.operator}{item.code})", item.kind)


@dataclass(frozen=True)
class BinaryNode(Node):
    operator: str
    left: Node
    right: Node

    def emit(self, context: "EmitContext") -> Emitted:
        left = self.left.emit(context)
        right = self.right.emit(context)
        if self.operator in {"=", "<>", ">", "<", ">=", "<="}:
            if left.kind == "number" and isinstance(self.right, StringNode):
                try:
                    right = Emitted(cs_decimal(self.right.value), "number")
                except InvalidOperation:
                    pass
            elif right.kind == "number" and isinstance(self.left, StringNode):
                try:
                    left = Emitted(cs_decimal(self.left.value), "number")
                except InvalidOperation:
                    pass
            operator = "==" if self.operator == "=" else "!=" if self.operator == "<>" else self.operator
            return Emitted(f"({left.code} {operator} {right.code})", "bool")
        if self.operator == "^":
            return Emitted(f"LegacyMath.Power({left.code}, {right.code})", "number")
        return Emitted(f"({left.code} {self.operator} {right.code})", "number")


@dataclass(frozen=True)
class FunctionNode(Node):
    name: str
    arguments: tuple[Node, ...]

    def emit(self, context: "EmitContext") -> Emitted:
        name = self.name.upper()
        if name == "IF" and len(self.arguments) == 3:
            condition = self.arguments[0].emit(context)
            when_true = self.arguments[1].emit(context)
            when_false = self.arguments[2].emit(context)
            return Emitted(f"({condition.code} ? {when_true.code} : {when_false.code})", when_true.kind)
        if name == "ROUNDUP" and len(self.arguments) == 2:
            value = self.arguments[0].emit(context)
            digits = self.arguments[1]
            if not isinstance(digits, NumberNode):
                raise ValueError("ROUNDUP digits must be a literal")
            return Emitted(f"ExcelMath.RoundUp({value.code}, {int(Decimal(digits.value))})", "number")
        if name == "SUM":
            values = [argument.emit(context) for argument in self.arguments]
            return Emitted("(" + " + ".join(item.code for item in values) + ")", "number")
        if name == "COUNTIF" and len(self.arguments) == 2:
            range_node, criterion = self.arguments
            if not isinstance(range_node, RangeNode) or not isinstance(criterion, StringNode):
                raise ValueError("Only the legacy layer COUNTIF shape is supported")
            if range_node.start.sheet != "BİNA BİLGİLERİ" or range_node.start.coordinate != "D32" or range_node.end.coordinate != "F35":
                raise ValueError(f"Unsupported COUNTIF range: {range_node}")
            return Emitted(f"c.CountLayers({cs_string(stable_code(criterion.value))})", "number")
        raise ValueError(f"Unsupported function {self.name}/{len(self.arguments)}")


class FormulaParser:
    def __init__(self, formula: str):
        text = formula[1:] if formula.startswith("=") else formula
        self.tokens: list[Token] = []
        position = 0
        while position < len(text):
            match = TOKEN_PATTERN.match(text, position)
            if not match:
                raise ValueError(f"Unexpected formula token near: {text[position:position+30]!r} in {formula}")
            position = match.end()
            if match.lastgroup != "SPACE":
                self.tokens.append(Token(match.lastgroup or "", match.group()))
        self.position = 0

    def parse(self) -> Node:
        node = self.parse_comparison()
        if self.position != len(self.tokens):
            raise ValueError(f"Unexpected trailing token {self.tokens[self.position:]}")
        return node

    def parse_comparison(self) -> Node:
        node = self.parse_additive()
        while self.peek_text() in {"=", "<>", ">", "<", ">=", "<="}:
            operator = self.take().text
            node = BinaryNode(operator, node, self.parse_additive())
        return node

    def parse_additive(self) -> Node:
        node = self.parse_multiplicative()
        while self.peek_text() in {"+", "-"}:
            operator = self.take().text
            node = BinaryNode(operator, node, self.parse_multiplicative())
        return node

    def parse_multiplicative(self) -> Node:
        node = self.parse_power()
        while self.peek_text() in {"*", "/"}:
            operator = self.take().text
            node = BinaryNode(operator, node, self.parse_power())
        return node

    def parse_power(self) -> Node:
        node = self.parse_unary()
        while self.peek_text() == "^":
            self.take()
            node = BinaryNode("^", node, self.parse_unary())
        return node

    def parse_unary(self) -> Node:
        if self.peek_text() in {"+", "-"}:
            return UnaryNode(self.take().text, self.parse_unary())
        return self.parse_primary()

    def parse_primary(self) -> Node:
        token = self.take()
        if token.kind == "NUMBER":
            return NumberNode(token.text)
        if token.kind == "STRING":
            return StringNode(token.text[1:-1].replace('""', '"'))
        if token.kind in {"CELL", "SHEETCELL"}:
            node = self.cell_from_token(token)
            if self.peek_text() == ":":
                self.take()
                end_token = self.take()
                if end_token.kind not in {"CELL", "SHEETCELL"}:
                    raise ValueError("Range endpoint must be a cell")
                end = self.cell_from_token(end_token)
                if end.sheet is None:
                    end = CellNode(node.sheet, end.coordinate)
                return RangeNode(node, end)
            return node
        if token.kind == "IDENT":
            if self.peek_text() != "(":
                raise ValueError(f"Identifier without function call: {token.text}")
            self.take()
            arguments: list[Node] = []
            if self.peek_text() != ")":
                while True:
                    arguments.append(self.parse_comparison())
                    if self.peek_text() != ",":
                        break
                    self.take()
            self.expect(")")
            return FunctionNode(token.text, tuple(arguments))
        if token.text == "(":
            node = self.parse_comparison()
            self.expect(")")
            return node
        raise ValueError(f"Unexpected primary token: {token}")

    @staticmethod
    def cell_from_token(token: Token) -> CellNode:
        if token.kind == "SHEETCELL":
            sheet, coordinate = token.text.rsplit("!", 1)
            sheet = sheet[1:-1].replace("''", "'")
            return CellNode(sheet, coordinate.replace("$", ""))
        return CellNode(None, token.text.replace("$", ""))

    def peek_text(self) -> str | None:
        return self.tokens[self.position].text if self.position < len(self.tokens) else None

    def take(self) -> Token:
        if self.position >= len(self.tokens):
            raise ValueError("Unexpected end of formula")
        token = self.tokens[self.position]
        self.position += 1
        return token

    def expect(self, text: str) -> None:
        token = self.take()
        if token.text != text:
            raise ValueError(f"Expected {text}, got {token.text}")


class EmitContext:
    def __init__(self, material_by_row: dict[int, dict[str, Any]]):
        self.material_by_row = material_by_row

    def emit_cell(self, cell: CellNode) -> Emitted:
        coordinate = cell.coordinate
        if cell.sheet == "BİNA BİLGİLERİ":
            if coordinate in NUMERIC_INPUTS:
                return Emitted(f"c.Number(LegacyInputField.{NUMERIC_INPUTS[coordinate]})", "number")
            if coordinate in TEXT_INPUTS:
                return Emitted(f"c.Option(LegacyInputField.{TEXT_INPUTS[coordinate]})", "string")
            raise ValueError(f"Unmapped building input: {coordinate}")
        if cell.sheet is not None:
            raise ValueError(f"Unsupported sheet reference: {cell.sheet}!{coordinate}")
        if coordinate in PRICING_CELLS:
            return Emitted(f"c.Pricing.{PRICING_CELLS[coordinate]}", "number")
        if coordinate == "Q82":
            return Emitted("c.WindowTrimQuantity()", "number")
        column, row = re.fullmatch(r"([A-Z]+)(\d+)", coordinate).groups()
        row_number = int(row)
        material = self.material_by_row.get(row_number)
        if material and column == "D":
            return Emitted(f"c.Quantity({cs_string(material['code'])})", "number")
        if material and column == "C":
            return Emitted(f"c.SpecificationNumber({cs_string(material['code'])})", "number")
        if material and column == "I":
            return Emitted(f"c.PurchaseUnitPriceExVat({cs_string(material['code'])})", "number")
        if material and column == "K":
            return Emitted(f"c.PurchaseUnitPriceIncVat({cs_string(material['code'])})", "number")
        if material and column == "E":
            return Emitted(f"c.SalesUnitPrice({cs_string(material['code'])})", "number")
        if material and column == "F":
            return Emitted(f"c.SalesLineTotal({cs_string(material['code'])})", "number")
        if column == "H" and 154 <= row_number <= 173:
            linked = self.material_by_row[row_number]
            return Emitted(f"c.Parameter({cs_string('PLUMBING_COEFFICIENT:' + linked['code'])})", "number")
        if column in {"X", "Y"}:
            return Emitted(f"c.Parameter({cs_string('LEGACY_OPTIONAL:' + coordinate)})", "number")
        raise ValueError(f"Unmapped local cell: {coordinate}")

    def emit_range(self, range_node: RangeNode) -> Emitted:
        start = range_node.start
        end = range_node.end
        if start.sheet == "BİNA BİLGİLERİ" and start.coordinate == "B61" and end.coordinate == "F64":
            return Emitted("c.SumFixtureCounts()", "number")
        if start.sheet is None and start.coordinate.startswith("D") and end.coordinate.startswith("D"):
            first = int(start.coordinate[1:])
            last = int(end.coordinate[1:])
            codes = [self.material_by_row[row]["code"] for row in range(first, last + 1) if row in self.material_by_row]
            return Emitted("c.SumQuantities(" + ", ".join(cs_string(code) for code in codes) + ")", "number")
        if start.sheet == "BİNA BİLGİLERİ" and start.coordinate == "D32" and end.coordinate == "F35":
            return Emitted("c.LayerCount", "range")
        raise ValueError(f"Unsupported range: {start.sheet}!{start.coordinate}:{end.coordinate}")


def formula_expression(formula: str | None, constant: Any, context: EmitContext) -> str:
    if formula:
        return FormulaParser(formula).parse().emit(context).code
    return cs_decimal(constant)


def generate() -> None:
    materials: list[dict[str, Any]] = json.loads(CATALOG_PATH.read_text(encoding="utf-8"))
    material_by_row = {int(item["excelRow"]): item for item in materials}
    context = EmitContext(material_by_row)

    workbook = load_workbook(next(ROOT.glob("*.xlsx")), data_only=True, read_only=False)
    offer = workbook.worksheets[1]
    parameters: list[tuple[str, str, str, Any]] = []
    for row in range(154, 174):
        material = material_by_row[row]
        parameters.append(
            (f"PLUMBING_COEFFICIENT:{material['code']}", material["code"], "Sıhhi tesisat yardımcı katsayısı", offer[f"H{row}"].value or 0)
        )
    for coordinate in ["X94", "Y94", "X138", "Y138", "X146", "Y146", "X147", "Y147", "X148", "Y148", "X149", "Y149"]:
        row = int(re.search(r"\d+", coordinate).group())
        parameters.append(
            (f"LEGACY_OPTIONAL:{coordinate}", material_by_row[row]["code"], f"Legacy opsiyonel parametre {coordinate}", offer[coordinate].value or 0)
        )

    definition_lines: list[str] = []
    quantity_lines: list[str] = []
    purchase_lines: list[str] = []
    sales_lines: list[str] = []
    errors: list[str] = []

    for item in materials:
        specification_number = item["specification"]
        definition_lines.append(
            "        new(" + ", ".join(
                [
                    cs_string(item["code"]),
                    str(item["categoryId"]),
                    cs_string(item["category"]),
                    str(item["sortOrder"]),
                    str(item["excelRow"]),
                    cs_string(item["name"]),
                    cs_string(str(item["specification"]) if item["specification"] is not None else None),
                    cs_string(item["unit"]),
                    cs_decimal(specification_number),
                    cs_string(item["quantityRuleId"]),
                    cs_string(item["pricingRuleId"]),
                ]
            ) + "),"
        )
        try:
            quantity = formula_expression(item["quantityFormula"], item["quantityConstant"], context)
            purchase = formula_expression(item["purchasePriceFormula"], item["purchasePriceConstant"], context)
            sales = formula_expression(item["salesPriceFormula"], item["salesPriceConstant"], context)
            # Excel's 1.73 sales coefficient is a project pricing parameter. Keeping
            # the substitution in the generator makes regenerated rules editable too.
            sales = sales.replace("1.73m", "c.Pricing.SalesMarkupFactor")
            quantity_lines.append(f"        {cs_string(item['code'])} => {quantity},")
            purchase_lines.append(f"        {cs_string(item['code'])} => {purchase},")
            sales_lines.append(f"        {cs_string(item['code'])} => {sales},")
        except Exception as exc:
            errors.append(f"{item['code']} row {item['excelRow']}: {exc}")

    if errors:
        raise RuntimeError("\n".join(errors))

    parameter_lines = [
        f"        new({cs_string(identifier)}, {cs_string(material_code)}, {cs_string(name)}, {cs_decimal(value)}, FormulaVersion),"
        for identifier, material_code, name, value in parameters
    ]

    content = "\n".join(
        [
            "// <auto-generated />",
            "// Source: Legacy/MaterialCatalog.json produced from the immutable legacy workbook.",
            "using ZMT.SteelCost.Domain;",
            "",
            "namespace ZMT.SteelCost.Application.Calculation;",
            "",
            "public static class LegacyExcelV1Rules",
            "{",
            '    public const string FormulaVersion = "LegacyExcel-v1";',
            "",
            "    public static IReadOnlyList<LegacyMaterialDefinition> Materials { get; } =",
            "    [",
            *definition_lines,
            "    ];",
            "",
            "    public static IReadOnlyList<MaterialFormulaParameter> FormulaParameters { get; } =",
            "    [",
            *parameter_lines,
            "    ];",
            "",
            "    public static decimal CalculateQuantity(string materialCode, LegacyRuleContext c) => materialCode switch",
            "    {",
            *quantity_lines,
            '        _ => throw new KeyNotFoundException($"Miktar kuralı bulunamadı: {materialCode}")',
            "    };",
            "",
            "    public static decimal CalculatePurchaseUnitPriceExVat(string materialCode, LegacyRuleContext c) => materialCode switch",
            "    {",
            *purchase_lines,
            '        _ => throw new KeyNotFoundException($"Alış fiyat kuralı bulunamadı: {materialCode}")',
            "    };",
            "",
            "    public static decimal CalculateSalesUnitPrice(string materialCode, LegacyRuleContext c) => materialCode switch",
            "    {",
            *sales_lines,
            '        _ => throw new KeyNotFoundException($"Satış fiyat kuralı bulunamadı: {materialCode}")',
            "    };",
            "}",
            "",
        ]
    )
    OUTPUT_PATH.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT_PATH.write_text(content, encoding="utf-8")
    print(f"Generated {OUTPUT_PATH}")
    print(f"Material rules: {len(materials)} quantity + {len(materials)} purchase + {len(materials)} sales")
    print(f"Formula parameters: {len(parameters)}")


if __name__ == "__main__":
    generate()
