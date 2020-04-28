var finalestats = 1
var formulari = false
var idPut
generarContingut();
crearOportunitats();
generarClients();
function generarContingut() {
    xmlHttpHandler('Get', 'estat')
        .then((response) => {

            var estats = JSON.parse(response);
            for (i = 0; i < estats.length; i++) {
                let section = document.createElement("section");
                const content = document.querySelector("#section1");
                section.setAttribute("id", "col" + (i + 1));
                content.appendChild(section);

                let div = document.createElement("div");
                const content2 = document.querySelector("#col" + (i + 1));
                div.setAttribute("class", "row1");
                content2.appendChild(div);
                div.textContent = estats[i].Nom;


                let div2 = document.createElement("div");
                const content3 = document.querySelector("#col" + (i + 1));
                div2.setAttribute("id", "" + "Estat" + estats[i].ID);
                div2.setAttribute("class", "row2");
                div2.setAttribute("ondrop", "onDrop(event)");
                div2.setAttribute("ondragover", "allowDrop(event)");
                content3.appendChild(div2);

                //Selector
                var mySelect = document.getElementById("estat");
                var myOption = document.createElement("option");
                myOption.setAttribute("value", estats[i].ID);
                myOption.setAttribute("label", estats[i].Nom);
                mySelect.appendChild(myOption);
            }
        })
        .catch((error) => {
            console.log(error);
        });
}

function crearOportunitats() {
    xmlHttpHandler('Get', 'oportunitat')
        .then((response) => {
            var oportunitats = JSON.parse(response);

            for (r = 0; r < oportunitats.length; r++) {
                let article = document.createElement("article");
                const content4 = document.getElementById("Estat" + oportunitats[r].Estat);
                article.textContent = oportunitats[r].Nom;
                article.setAttribute("id", "Op" + oportunitats[r].ID);
                article.setAttribute("draggable", "true");
                article.setAttribute("ondragstart", "drag(event)");
                article.setAttribute("ondragend", "onDragEnd(event)");
                article.setAttribute("ondblclick", "onDoubleClick(event)");
                content4.appendChild(article);
                if (r == oportunitats.length - 1) {
                    finalestats = oportunitats[oportunitats.length - 1].ID + 1;
                }
            }

        })
        .catch((error) => {
            console.log(error);
        });
}

function generarClients() {
    xmlHttpHandler('Get', 'client')
        .then((response) => {
            var client = JSON.parse(response);

            for (i = 0; i < client.length; i++) {
                //Selector
                var mySelect = document.getElementById("client");
                var myOption = document.createElement("option");
                myOption.setAttribute("value", client[i].ID);
                myOption.setAttribute("label", " ID: " + client[i].ID + " | Nom: " + client[i].Nom);
                mySelect.appendChild(myOption);
            }
        })
        .catch((error) => {
            console.log(error);
        });
}

function allowDrop(ev) {
    ev.preventDefault();

}

function drag(ev) {
    var draggingElement = ev.target;
    ev.dataTransfer.setData("text", ev.target.id);
    draggingElement.style.opacity = '0.4';
}

function onDrop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.getData("text");
    var estat = ev.target.id.substring(5);

    var dades = {
        Estat: estat,
    }

    xmlHttpHandler('PUT', 'oportunitat/Usuari1/' + data.substring(2), dades)
        .then((response) => {
            ev.target.appendChild(document.getElementById(data));
        })
        .catch((error) => {
            console.log(error);
        });
}

function onDragEnd(ev) {
    var draggingElement = ev.target;
    draggingElement.style.opacity = '1';
}

function onDelete(ev) {
    ev.preventDefault();
    var r = confirm("Segur que vols marcar aquesta oportunitat com a finalitzada?");
    if (r == true) {
        var data = ev.dataTransfer.getData("Text");
        var el = document.getElementById(data);

        var dades = {
            Acabat: 1,
        }

        xmlHttpHandler('PUT', 'oportunitat/Usuari1/' + data.substring(2), dades)
            .then((response) => {
                el.parentNode.removeChild(el);
            })
            .catch((error) => {
                console.log(error);
            });
    }
}

function onDelete2(ev) {
    ev.preventDefault();
    var r = confirm("Segur que vols arxivar aquesta oportunitat?");
    if (r == true) {
        var data = ev.dataTransfer.getData("Text");
        var el = document.getElementById(data);

        var dades = {
            Acabat: 2,
        }

        xmlHttpHandler('PUT', 'oportunitat/Usuari1/' + data.substring(2), dades)
            .then((response) => {
                el.parentNode.removeChild(el);
            })
            .catch((error) => {
                console.log(error);
            });
    }
}


function openForm() {
    formulari = true;
    document.getElementById("titol").innerHTML = "Afegir Oportunitat";
    document.getElementById("afegirButton").innerHTML = "Afegir";
    document.getElementById("input").value = "";
    document.getElementById("desc").value = "";
    document.getElementById("estat").selectedIndex = "0"
    document.getElementById("client").selectedIndex = "0"
    document.getElementById("myForm").classList.add("active");
    document.getElementById("Info").style.display = "none";
    document.getElementById("Afegir").style.display = "block";
    document.querySelectorAll(".tab").forEach((element) => {
        element.style.display = "none";
    });
}

function closeForm() {
    document.getElementById("myForm").classList.remove("active");
}

function onButton() {
    var nom = document.getElementById("input").value;
    var client = document.getElementById("client").value;
    var estat = document.getElementById("estat").value;
    var desc = document.getElementById("desc").value;

    if (nom != "") {
        if (formulari == true) {
            let article = document.createElement("article");
            const content = document.getElementById("Estat" + estat);
            article.textContent = nom;
            article.setAttribute("draggable", "true");
            article.setAttribute("ondragstart", "drag(event)");
            article.setAttribute("ondragend", "onDragEnd(event)");
            article.setAttribute("id", "Op" + finalestats);
            article.setAttribute("ondblclick", "onDoubleClick(event)");

            var dades = {
                Nom: nom,
                Descripcio: desc,
                Client_ID: client,
                Estat: estat,
            }

            xmlHttpHandler('POST', 'oportunitat/Usuari1', dades)
                .then((response) => {
                    finalestats++;
                    content.appendChild(article);
                    document.getElementById("myForm").classList.remove("active");
                })
                .catch((error) => {
                    console.log(error);
                });
        } else {
            var dades = {
                Nom: nom,
                Descripcio: desc,
                Client_ID: client,
                Estat: estat,
            }
            xmlHttpHandler('PUT', 'oportunitat/Usuari1/' + idPut, dades)
                .then((response) => {
                    document.getElementById("Estat" + estat).appendChild(document.getElementById("Op" + idPut));
                    document.getElementById("Op" + idPut).textContent = nom;
                    document.getElementById("myForm").classList.remove("active");
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    }
}

function openForm2(evt, formName) {
    var i, tabcontent;
    // Desactivar tot lo anterior
    tabcontent = document.getElementsByClassName("form-container");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Activar tab i contingut 
    document.getElementById(formName).style.display = "block";
    document.querySelectorAll(".tab").forEach((element) => {
        element.style.display = "flex";
    });
}

function onDoubleClick(ev) {
    formulari = false;
    idPut = ev.target.id.substring(2);
    $("#myModal").modal('show');

    //Get de l'oportunitat
    xmlHttpHandler('GET', 'oportunitat/' + idPut)
        .then((response) => {
            var myData = JSON.parse(response);
            document.getElementById("input2").value = myData.Nom;
            document.getElementById("desc2").value = myData.Descripcio;

            //Get el nom de l'estat
            xmlHttpHandler('GET', 'estat/' + myData.Estat)
                .then((response2) => {
                    var myData2 = JSON.parse(response2);
                    document.getElementById("estat2").value = myData2[0].Nom;
                })
                .catch((error) => {
                    console.log(error);
                });

            //Get el nom del client
            xmlHttpHandler('GET', 'client/' + myData.Client_ID)
                .then((response2) => {
                    var myData2 = JSON.parse(response2);
                    document.getElementById("client2").value = "ID: " + myData.Client_ID + " | Nom: " + myData2[0].Nom;
                })
                .catch((error) => {
                    console.log(error);
                });

            document.getElementById("input").value = myData.Nom;
            document.getElementById("desc").value = myData.Descripcio;
            document.getElementById("estat").selectedIndex = myData.Estat;
            document.getElementById("client").value = myData.Client_ID;
        })
        .catch((error) => {
            console.log(error);
        });

}

function xmlHttpHandler(type, params, FromData = null) {
    var global_xhh = null;
    return new Promise((resolve, reject) => {
        if (global_xhh == null) {
            global_xhh = new XMLHttpRequest();
        } else {
            global_xhh.abort();
        }

        if (!FromData) {
            var url = "https://localhost:44300/" + params;

            global_xhh.open(type, url, true);
            global_xhh.onload = () => resolve(global_xhh.responseText);
            global_xhh.onerror = () => reject(global_xhh.statusText);
            global_xhh.send();
        } else {
            var url = "https://localhost:44300/" + params;
            global_xhh.open(type, url, true);
            global_xhh.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            global_xhh.onload = () => resolve(global_xhh.responseText);
            global_xhh.onerror = () => reject(global_xhh.statusText);

            global_xhh.send("pepito=" + JSON.stringify(FromData));
        }

    });
}


