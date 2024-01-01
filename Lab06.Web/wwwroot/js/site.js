const elForms = document.querySelectorAll(".ticket-form");
const elBtnSubmits = document.querySelectorAll(".ticket-form-btnSubmit");
const elTickets = document.querySelectorAll(".ticket-field-input");
const spinners = document.querySelectorAll('.spinner-container');

let ticketsCount = 0;
let isSubmitting = false;

const checkTicketsValidation = () => ticketsCount >= 1 && ticketsCount <= 9;

elBtnSubmits.forEach((btnSubmit) => {
    btnSubmit.addEventListener("click", handleBtnClick);
});

elTickets.forEach((ticket) => {
    ticket.addEventListener("keyup", handleCountChange);
    ticket.addEventListener("change", handleCountChange);
});

elForms.forEach((ticketForm) => {
    ticketForm.addEventListener("submit", handleFormSubmit);
});

function handleCountChange(event) {
    let ticketInputFullId = event.target.id;
    let ticketId = ticketInputFullId.substring(ticketInputFullId.lastIndexOf('-')).slice(1);
    let currentBtnSubmit = document.getElementById('btnSubmit-' + ticketId);

    ticketsCount = Number(event.target.value);

    if (checkTicketsValidation()) {
        currentBtnSubmit.removeAttribute("disabled");
    } else {
        currentBtnSubmit.setAttribute("disabled", "");
    }
}

function handleBtnClick(event) { }

async function handleFormSubmit(event) {
    event.preventDefault(); // avoid native form submit (page refresh)

    let formFullId = event.target.id;
    let ticketId = formFullId.substring(formFullId.lastIndexOf('-')).slice(1);
    let currentBtnSubmit = document.getElementById('btnSubmit-' + ticketId);
    let currentSpinner = document.getElementById('spinner-' + ticketId);
    let currentTicketInput = document.getElementById('ticket-input-' + ticketId);

    if (isSubmitting) {
        console.log("Double submit prevented");
        return false;
    }

    isSubmitting = true;
    currentBtnSubmit.style.display = "none";
    currentSpinner.style.display = "block";

    let ticketInputValue = currentTicketInput.value;
    ajaxPurchase(ticketId, ticketInputValue).then(response => {
        currentSpinner.style.display = "none";
        currentBtnSubmit.style.display = "block";
        currentBtnSubmit.setAttribute("disabled", "");
        currentTicketInput.value = '';
    });


    isSubmitting = false;
}

function ajaxPurchase(ticketId, inputValue) {
    return $.ajax({
        type: "POST",
        url: "/Ticket/Purchase",
        data: {
            parkAttractionId: ticketId,
            numberOfTickets: inputValue
        }
    });
}