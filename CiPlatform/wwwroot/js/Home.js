function showList(e) {
    var $gridCont = $('.grid-container');
    e.preventDefault();
    $gridCont.addClass('listview');
}
function gridList(e) {
    var $gridCont = $('.grid-container')
    e.preventDefault();
    $gridCont.removeClass('listview');
}

$(document).on('click', '.btn-grid', gridList);
$(document).on('click', '.btn-list', showList);


function.onclick = () => {
    const myNode = document.querySelector(".filters-section");
    while (myNode.lastElementChild) {
        myNode.removeChild(myNode.lastElementChild);
    }


}
// Filter

let filtersSection = document.querySelector(".filters-section");
var filterList = document.querySelector(".filter-list");
var cbs = document.querySelectorAll('input[type=checkbox]');
for (var i = 0; i < cbs.length; i++) {
    cbs[i].addEventListener('change', function () {
        if (this.checked) {
            addElement(this, this.value);
        }
        else {

            removeElement(this.value);
            console.log("unchecked");
        }
    });
}


clearbutton.onclick = () => {
    const myNode = document.querySelector(".filters-section");

    while (myNode.lastElementChild) {
        myNode.removeChild(myNode.lastElementChild);
    }
    for (const checkbox of document.querySelectorAll('input[type=checkbox]')) {


        checkbox.checked = false //for unselection
        document.getElementById("ClearBtn").style.display = "none";
    }

}


function addElement(current, value) {
    let filtersSection = document.querySelector(".filters-section");
    let clearAllButton = document.getElementById("ClearBtn");
    let createdTag = document.createElement('span');
    createdTag.classList.add('filter-list');
    createdTag.classList.add('ps-3');
    createdTag.classList.add('pe-1');
    createdTag.classList.add('me-2');
    createdTag.innerHTML = value;

    createdTag.setAttribute('id', value);
    let crossButton = document.createElement('button');
    crossButton.classList.add("filter-close-button");
    let cross = '&times;'


    crossButton.addEventListener('click', function () {
        let elementToBeRemoved = document.getElementById(value);

        console.log(elementToBeRemoved);
        console.log(current);
        elementToBeRemoved.remove();

        current.checked = false;

    })

    crossButton.innerHTML = cross;
    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);

    if (filtersSection.hasChildNodes()) {

        document.getElementById("ClearBtn").style.display = "block";
    } else {
        document.getElementById("ClearBtn").style.display = "none";
    }

}
function ClearAllElement() {

    var filtersSection = document.querySelector(".filters-section");

    for (var i = 0; i < filtersSection.length; i++) {
        filtersSection.pop();
    }


}


function removeElement(value) {

    let filtersSection = document.querySelector(".filters-section");
    let clearAllButton = document.getElementById("ClearBtn");
    let elementToBeRemoved = document.getElementById(value);
    document.getElementById(value).checked = false;
    filtersSection.removeChild(elementToBeRemoved);
    if (filtersSection.hasChildNodes()) {
        document.getElementById("ClearBtn").style.display = "block";
    } else {

        document.getElementById("ClearBtn").style.display = "none";
    }

}

const items = document.querySelectorAll('#myItems');
const itemsPerPage = 4;

const totalPages = Math.ceil(items.length / itemsPerPage);
console.log(items);
function displayPage(pageNumber) {
    const startIndex = (pageNumber - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;

    items.forEach((item, index) => {
        if (index >= startIndex && index < endIndex) {
            item.style.display = 'block';
        } else {
            item.style.display = 'none';
        }
    });
}

function createPaginationLinks() {
    const pagination = document.querySelector('#pagination .pagination');

    for (let i = 1; i <= totalPages; i++) {
        const listItem = document.createElement('li');
        listItem.classList.add('page-item');

        const link = document.createElement('a');
        link.href = '#';
        link.textContent = i;
        link.classList.add('page-link');
        link.addEventListener('click', (event) => {
            event.preventDefault();
            displayPage(i);
        });
        listItem.appendChild(link);
        pagination.appendChild(listItem);
    }
}

displayPage(1);
createPaginationLinks();