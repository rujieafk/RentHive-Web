/*dropdown*/
document.addEventListener("DOMContentLoaded", function () {
    var dropdown = document.querySelector('.dropdown');

    dropdown.addEventListener('click', function () {
        dropdown.classList.toggle('active');
    });
});

/* Password unhide*/
document.addEventListener("DOMContentLoaded", function () {
    const passwordInput = document.querySelector('.password-text');
    const showIcon = document.getElementById('showIcon');
    const hideIcon = document.getElementById('hideIcon');

    showIcon.addEventListener("click", () => {
        passwordInput.type = 'text';
        showIcon.style.display = 'none';
        hideIcon.style.display = 'inline';
    });

    hideIcon.addEventListener("click", () => {
        passwordInput.type = 'password';
        showIcon.style.display = 'inline';
        hideIcon.style.display = 'none';
    });
});

/* Update Profile user*/
document.addEventListener("DOMContentLoaded", () => {
    const editButton = document.querySelector(".editButton");
    const saveButton = document.querySelector(".saveButton");

    // Select the input fields by their IDs
    const userDisplayName = document.querySelector("#userDisplayName");
    const userFirstName = document.querySelector("#userFirstName");
    const userMiddleName = document.querySelector("#userMiddleName");
    const userLastName = document.querySelector("#userLastName");
    const userPhoneNum = document.querySelector("#userPhoneNum");
    const userBirthDate = document.querySelector("#userBirthDate");
    const userAddress = document.querySelector("#userAddress");
    const userEnail = document.querySelector("#userEmail");
    const userPassword = document.querySelector("#userPassword");

    editButton.addEventListener("click", () => {
        // Remove the readonly attribute
        userDisplayName.removeAttribute("readonly");
        userFirstName.removeAttribute("readonly");
        userMiddleName.removeAttribute("readonly");
        userLastName.removeAttribute("readonly");
        userPhoneNum.removeAttribute("readonly");
        userBirthDate.removeAttribute("readonly");
        userAddress.removeAttribute("readonly");
        userEnail.removeAttribute("readonly");
        userPassword.removeAttribute("readonly");

        // Toggle button visibility
        editButton.style.display = "none";
        saveButton.style.display = "inline-block";
    });

    saveButton.addEventListener("click", () => {
        // Add the readonly attribute back
        userDisplayName.setAttribute("readonly", "readonly");
        userFirstName.setAttribute("readonly", "readonly");
        userMiddleName.setAttribute("readonly", "readonly");
        userLastName.setAttribute("readonly", "readonly");
        userPhoneNum.setAttribute("readonly", "readonly");
        userBirthDate.setAttribute("readonly", "readonly");
        userAddress.setAttribute("readonly", "readonly");
        userEmail.setAttribute("readonly", "readonly");
        userPassword.setAttribute("readonly", "readonly");

        // Toggle button visibility
        editButton.style.display = "inline-block";
        saveButton.style.display = "none";
    });
});

/*Searching*/
function searchById() {
    var input, filter, table, tr, id, user, i, txtValue;

    input = document.getElementById("searchId");
    filter = input.value.toUpperCase();
    table = document.querySelector(".table-content table");
    tr = table.getElementsByTagName("tr");

    for (i = 1; i < tr.length; i++) {
        user = tr[i].getElementsByTagName("td")[0];
        id = tr[i].getElementsByTagName("td")[1];
        topic = tr[i].getElementsByTagName("td")[2];

        if (id && user && topic) {
            // Check against both id and user
            var idText = id.textContent || id.innerText;
            var userText = user.textContent || user.innerText;
            var topicText = topic.textContent || topic.innerText;

            if (idText.toUpperCase().indexOf(filter) > -1 || userText.toUpperCase().indexOf(filter) > -1 || topicText.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}






/*DELETE LATER*/
/*document.addEventListener("DOMContentLoaded", function () {
    const startDateInput = document.getElementById("startDate");
    const endDateInput = document.getElementById("endDate");
    const resultInput = document.getElementById("result");
    const timeInput = document.getElementById("setTimeBan");

    startDateInput.addEventListener("input", calculateRemainingDays);
    endDateInput.addEventListener("input", calculateRemainingDays);

    function calculateRemainingDays() {
        const startDate = new Date(startDateInput.value + "T00:00:00"); // Set time to midnight
        const endDate = new Date(endDateInput.value + "T00:00:00");     // Set time to midnight
        const currentDate = new Date();

        if (startDate > endDate) {
            resultInput.value = "End date cannot be before the start date.";
        } else {
            const timeDifference = endDate - currentDate;
            const remainingDays = Math.ceil(timeDifference / (1000 * 60 * 60 * 24));

            if (remainingDays < 0) {
                resultInput.value = "The end date is in the past.";
            } else {
                resultInput.value = remainingDays;
                timeInput.value = remainingDays;
            }
        }
    }
});*/




    /*document.addEventListener("DOMContentLoaded", function () {
        document.getElementById("dateFilterForm").addEventListener("submit", function (event) {
            event.preventDefault();

            var startDate = new Date(document.getElementById("startDate").value).toISOString().split('T')[0];
            var endDate = new Date(document.getElementById("endDate").value).toISOString().split('T')[0];

            filterByDateRange(startDate, endDate);
        });

        function filterByDateRange(startDate, endDate) {
            var tableRows = document.querySelectorAll("#reportDetails table tr");

            for (var i = 1; i < tableRows.length; i++) {
                var rowDate = new Date(tableRows[i].querySelectorAll("td")[4].textContent).toISOString().split('T')[0]; // Assuming date is in the fifth column

                if (rowDate >= startDate && rowDate <= endDate) {
                    tableRows[i].style.display = ""; // Display the row if it falls within the date range
                } else {
                    tableRows[i].style.display = "none"; // Hide the row if it's outside the date range
                }
            }
        }
    });*/

//for prints
function printReport() {
    var printContents = document.getElementById('PrintFile').innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();

    document.body.innerHTML = originalContents;
}

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("dateFilterForm").addEventListener("submit", function (event) {
        event.preventDefault();

        // Convert the Date object to an ISO string, which includes date and time information
        // Split the ISO string at the "T" character, resulting in an array with date and time parts
        var startDate = new Date(document.getElementById("startDate").value).toISOString().split('T')[0];
        var endDate = new Date(document.getElementById("endDate").value).toISOString().split('T')[0];

        // Filter rows based on date range
        filterByDateRange(startDate, endDate);

        // Trigger the print dialog
        window.printReport();
    });

    function filterByDateRange(startDate, endDate) {
        var tableRows = document.querySelectorAll("#PrintFile table tbody tr");

        for (var i = 0; i < tableRows.length; i++) {
            var rowDate = new Date(tableRows[i].querySelectorAll("td")[3].textContent).toISOString().split('T')[0]; // example date is in fourth column

            if (rowDate >= startDate && rowDate <= endDate) {
                tableRows[i].style.display = ""; // Display the row if it falls within the date range
            } else {
                tableRows[i].style.display = "none"; // Hide the row if it's outside the date range
            }
        }
    }
    window.addEventListener("afterprint", function () {
        // Reload the page after printing is done or canceled
        location.reload();
    });
});
