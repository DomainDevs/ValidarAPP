$.ajaxSetup({ async: false });
function oEdge(Description, NodeId, QuestionId, ValueCode) {
    this.EdgeId = null;
    this.NodeId = NodeId;
    this.QuestionId = QuestionId;
    this.ScriptId = null;
    this.IsDefault = null;
    this.NextNodeId = null;
    this.Description = Description;       
    this.ValueCode = ValueCode;    
}

function oQuestion(QuestionId, Description) {    
    this.QuestionId = QuestionId;
    this.Description = Description;
    this.Edges = [];
    this.OrdenNum = 1;    
    //this.ConceptId = ConceptId;
    //this.EntityId = EntityId;
}

function oNode(NodeId, Questions, ScriptId) {
    this.NodeId = NodeId;
    this.ScriptId = ScriptId;
    this.Questions = Questions;
    //this.Questions = [];    
}

function oScript(Description, LevelId, PackageId, ScriptId) {
    this.ScriptId = ScriptId;
    this.Description = Description;
    this.LevelId = LevelId;
    this.LevelDescription = null;
    this.PackageId = PackageId;
    this.PackageDescription = null
    this.NodeId = 1;    
}

function oScriptComposite() {
    this.Script = null;
    this.Nodes = [];    
}