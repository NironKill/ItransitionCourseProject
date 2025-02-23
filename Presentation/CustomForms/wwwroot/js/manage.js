document.addEventListener("DOMContentLoaded", function () {
    const filterInput = document.getElementById("filter-input-user");
    const rows = document.querySelectorAll(".user-table tbody .user-row");
    const deleteButton = document.getElementById("delete-btn");
    const blockButton = document.getElementById("block-btn");
    const unblockButton = document.getElementById("unblock-btn");
    const privilegeButton = document.getElementById("privilege-btn");
    const deprivilegeButton = document.getElementById("deprivilege-btn");
    const disauthorizeYourselfButton = document.getElementById("admin-disauthorize-yourself");
    const selectAllCheckbox = document.getElementById("select-all");
    const userCheckboxes = document.querySelectorAll(".user-checkbox");
    let increment = document.querySelectorAll("#usersTable tbody tr");

    increment.forEach((row, index) => {
        let numberCell = row.querySelector(".user-number"); 
        if (numberCell) {
            numberCell.textContent = index + 1;
        }
    });

    filterInput.addEventListener("input", function () {
        const searchTerm = filterInput.value.toLowerCase();

        rows.forEach(row => {
            const name = row.querySelector(".user-name").textContent.toLowerCase();
            const email = row.querySelector(".user-email").textContent.toLowerCase();

            if (name.includes(searchTerm) || email.includes(searchTerm)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });
    });

    selectAllCheckbox.addEventListener("change", function () {
        const isChecked = this.checked;
        userCheckboxes.forEach(checkbox => checkbox.checked = isChecked);
    });

    function getSelectedEmails() {
        return Array.from(userCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);
    }

    deleteButton.addEventListener("click", async function () {
        const selectedEmails = getSelectedEmails();
        if (selectedEmails.length === 0) {
            alert("Please select at least one user to delete.");
            return;
        }

        const confirmed = confirm(`Are you sure you want to delete ${selectedEmails.length} user(s)?`);
        if (!confirmed) return;

        try {
            const response = await fetch("/AdminMenu/Delete", {
                method: "DELETE",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emails: selectedEmails }),
            });

            if (response.ok) {
                alert("Selected users deleted successfully!");
                selectedEmails.forEach(email => {
                    const row = document.querySelector(`.user-row .user-checkbox[value="${email}"]`).closest("tr");
                    if (row) row.remove();
                });
            } else {
                alert("Failed to delete users. Please try again.");
            }
        } catch (error) {
            console.error("Error deleting users:", error);
            alert("An error occurred while deleting users.");
        }
    });

    blockButton.addEventListener("click", async function () {
        const selectedEmails = getSelectedEmails();
        if (selectedEmails.length === 0) {
            alert("Please select at least one user to block.");
            return;
        }

        try {
            const response = await fetch("/AdminMenu/Lock", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emails: selectedEmails }),
            });

            if (response.ok) {
                alert("Selected users blocked successfully!");
                selectedEmails.forEach(email => {
                    const row = document.querySelector(`.user-row .user-checkbox[value="${email}"]`).closest("tr");
                    if (row) row.classList.add("locked");
                });
            } else {
                alert("Failed to block users. Please try again.");
            }
        } catch (error) {
            console.error("Error blocking users:", error);
            alert("An error occurred while blocking users.");
        }
    });

    unblockButton.addEventListener("click", async function () {
        const selectedEmails = getSelectedEmails();
        if (selectedEmails.length === 0) {
            alert("Please select at least one user to unblock.");
            return;
        }

        try {
            const response = await fetch("/AdminMenu/Unlock", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emails: selectedEmails }),
            });

            if (response.ok) {
                alert("Selected users unblocked successfully!");
                selectedEmails.forEach(email => {
                    const row = document.querySelector(`.user-row .user-checkbox[value="${email}"]`).closest("tr");
                    if (row) row.classList.remove("locked");
                });
            } else {
                alert("Failed to unblock users. Please try again.");
            }
        } catch (error) {
            console.error("Error unblocking users:", error);
            alert("An error occurred while unblocking users.");
        }
    });

    privilegeButton.addEventListener("click", async function () {
        const selectedEmails = getSelectedEmails();
        if (selectedEmails.length === 0) {
            alert("Please select at least one user to grant admin rights.");
            return;
        }

        try {
            const response = await fetch("/AdminMenu/Privilege", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emails: selectedEmails }),
            });

            if (response.ok) {
                alert("Admin rights granted successfully!");
                selectedEmails.forEach(email => {
                    const row = document.querySelector(`.user-row .user-checkbox[value="${email}"]`).closest("tr");
                    if (row) {
                        row.querySelector(".user-role").textContent = "Admin"; 
                    }
                });
            } else {
                alert("Failed to grant admin rights. Please try again.");
            }
        } catch (error) {
            console.error("Error granting admin rights:", error);
            alert("An error occurred while granting admin rights.");
        }
    });

    deprivilegeButton.addEventListener("click", async function () {
        const selectedEmails = getSelectedEmails();
        if (selectedEmails.length === 0) {
            alert("Please select at least one user to revoke admin rights.");
            return;
        }

        try {
            const response = await fetch("/AdminMenu/Deprivilege", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emails: selectedEmails }),
            });

            if (response.ok) {
                alert("Admin rights revoked successfully!");
                selectedEmails.forEach(email => {
                    const row = document.querySelector(`.user-row .user-checkbox[value="${email}"]`).closest("tr");
                    if (row) {
                        row.querySelector(".user-role").textContent = "User"; 
                    }
                });
            } else {
                alert("Failed to revoke admin rights. Please try again.");
            }
        } catch (error) {
            console.error("Error revoking admin rights:", error);
            alert("An error occurred while revoking admin rights.");
        }
    });

    disauthorizeYourselfButton.addEventListener("click", async function () {
        const response = await fetch("/AdminMenu/DeprivilegeYourself", {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });
        if (response.ok) {
            window.location.href = "/";
        } else {
            console.error("Failed to deprivilege.");
            alert("An error occurred. Please try again.");
        }
    });
}); 