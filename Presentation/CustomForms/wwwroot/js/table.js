document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll(".ticket-row");
    let increment = document.querySelectorAll("#ticketsTable tbody tr");

    increment.forEach((row, index) => {
        let numberCell = row.querySelector(".ticket-number");
        if (numberCell) {
            numberCell.textContent = index + 1;
        }
    });

    rows.forEach(row => {
        row.addEventListener("click", function () {
            window.location.href = row.getAttribute("data-href");
        });

        row.style.cursor = "pointer";
    });
});
