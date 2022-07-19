if (window.sessionStorage){
    let elements = document.querySelectorAll('[id]');

    for (let i = 0, length = elements.length; i < length; i++) {
        let name = elements[i].getAttribute('id');
   
        elements[i].value = sessionStorage.getItem(name) || elements[i].value;
   
        elements[i].onkeyup = function() {
            sessionStorage.setItem(name, elements[i].value);
        };
    }
}


function ValidCheckAdd(){
    let addName = document.getElementById('partName');
    let error = document.getElementById('validerror');

    if(addName.value === '')
    {
        if(!document.getElementById('content'))
        {
            error.style.display = "flex";
            error.style.justifyContent = "center";
        }else{
            error.style.display = "inline-flex";
        }
    }else{
        error.style.display = "none";
    }
}

function ValidCheckChange(){
    let newName = document.getElementById('newName');
    let error = document.getElementById('validerror');

    if(newName.value === '')
    {
        if(!document.getElementById('newContent'))
        {
            error.style.display = "flex";
            error.style.justifyContent = "center";
        }else{
            error.style.display = "inline-flex";
        }
    }else{
        error.style.display = "none";
    }
}


function RemoveStorage(){
    let elements = document.querySelectorAll('[id]');
   
    for (let i = 0, length = elements.length; i < length; i++) {
        let name = elements[i].getAttribute('id');
        sessionStorage.removeItem(name);
    }
}