const URL_BASE = "https://newton.now.sh/api/v2/";
const OPERATIONS = [
    "simplify", 
    "factor",
    "derive",
    "integrate",
    "zeroes",
    "tangent",
    "area",
    "cos",
    "sin",
    "tan",
    "arcos",
    "arcsin",
    "arctan",
    "abs",
    "log"
];

main();

function main() {
    fillSelectOperations();
}

function fillSelectOperations(){
    var sel = document.getElementById('operations');
    var fragment = document.createDocumentFragment();
    
    OPERATIONS.forEach(function(operation, index) {
        var opt = document.createElement('option');
        opt.innerHTML = operation;
        opt.value = operation;
        fragment.appendChild(opt);
    });
    
    sel.appendChild(fragment);
}

function sendRequest(operation, expression) {
    // TODO
}

function showResult(data) {
    var res = document.getElementById('result');
    res.innerHTML = data.result;
}

function calculate(){
 // TODO
}
