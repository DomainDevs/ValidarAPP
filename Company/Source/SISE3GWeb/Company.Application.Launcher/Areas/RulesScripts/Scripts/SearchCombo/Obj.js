$.ajaxSetup({ async: false });
function oLevel() {
    this.Description = null;
    this.LevelId = null;
    this.PackageId = null;
}

function oPackage() {
    this.Description = null;
    this.PackageId = null;
}

function oRuleSetComposite() {
    this.RuleSet = null;
    this.RuleComposites = [];
}

function oRuleSet() {
    this.RuleSetId = -1;
    this.PackageId = null;
    this.LevelId = null;
    this.Description = null;
    this.CurrentFrom = null;
    this.RuleSetVer = null;
    this.RuleSetXml = null;
    this.Package = null;
    this.Level = null;
}

function oRuleBase() {
    this.RuleBaseId = -1;
    this.Current = null;
    this.Description = null;
    this.LevelsDescription = null;
    this.PackageDescription = null;
    this.Published = null;
    this.LevelId = null;
    this.PackageId = null;
}

function oRuleComposite() {
    this.RuleId = -1;
    this.RuleName = null;
    this.Parameters = [];
    this.Conditions = [];
    this.Actions = [];
    this.Change = null;
}

function oAction() {
    this.Id = null;
    this.Expression = null;
    this.ActionType = null;
    this.ValueType = null;
    this.ConceptControl = null;    
    this.ConceptLeft = null;
    this.AssignType = null;
    this.Operator = {};
    this.ValueRight = null;
    this.ConceptRight = null;
    this.TemporalNameRight = null;
    this.TemporalNameLeft = null;
    this.InvokeType = null;
    this.IdFuction = null;
    this.DescriptionFunction = null;
    this.FunctionName = null;
    this.Message = null;
    this.RuleSetId = null;
    this.DescriptionRuleSet = null;
}

function oInvokeType() {
    this.Value = null;
}

function oActionType() {
    this.Code = null;
    this.Descripcion = null;
}

function oOperator() {
    this.OperatorCode = null;
    this.Description = null;
    this.Symbol = null;
    this.CodeBinaryOperatorType = null;
}

function oCondition() {
    this.Id = null;
    this.Expression = null;
    this.Concept = {};
    this.Comparator = {};
    this.Value = null;
    this.DescriptionValue = null;
    this.ValueType = null;
    this.ConceptControl = null;
    this.ConceptValue = {};
    this.RuleSetId = null;
}

function oConcept() {
    this.ConceptControlCode = null;
    this.ConceptId = null;
    this.ConceptName = null;
    this.Description = null;
    this.EntityId = null;
    this.IsNulleabe = null;
    this.IsPersistible = null;
    this.IsStatic = null;
    this.IsVisible = null;
    this.KeyOrder = null;
    this.OrderNum = null;
}

function oComparator() {
    this.ComparatorCode = null;
    this.Description = null;
    this.Symbol = null;
}

function oValueType() {
    this.Value = null;
    this.Description = null;
}