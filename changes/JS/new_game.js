



function newgame() {


    var main = $("#maingame");

    for (var i = 0; i < 25; i++) {
        addElement("maingame", "button", i + 1, null);

    }

}

function addElement(parentId, elementTag, elementId, html) {
    // Adds an element to the document
    var p = document.getElementById(parentId);
    var newElement = document.createElement(elementTag);
    var x = 60;
    var y = 60;
    newElement.setAttribute('id', elementId);
    newElement.innerHTML = html;
    newElement.setAttribute('position', 'absolute');
    newElement.setAttribute('top', x + "px");
    newElement.setAttribute('left', y + "px");
    p.appendChild(newElement);
}





