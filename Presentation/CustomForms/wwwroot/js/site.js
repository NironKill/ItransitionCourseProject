document.addEventListener("DOMContentLoaded", function () {
    const languageDropdown = document.getElementById("languageDropdown");

    if (!languageDropdown) return;

    const savedLanguage = localStorage.getItem("selectedLanguage") || "en";
    languageDropdown.value = savedLanguage;

    languageDropdown.addEventListener("change", function () {
        const selectedLanguage = this.value;
        const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);

        localStorage.setItem("selectedLanguage", selectedLanguage);

        fetch("/Home/CultureManagement", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: `culture=${encodeURIComponent(selectedLanguage)}&returnUrl=${returnUrl}`
        })
            .then(response => {
                if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
                window.location.reload();
            })
            .catch(error => console.error("Error changing language:", error));
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const themeSwitch = document.getElementById("themeSwitch");
    const htmlElement = document.documentElement;

    if (localStorage.getItem("theme") === "dark") {
        htmlElement.setAttribute("data-bs-theme", "dark");
        themeSwitch.checked = true;
    } else {
        htmlElement.setAttribute("data-bs-theme", "light");
        themeSwitch.checked = false;
    }

    themeSwitch.addEventListener("change", function () {
        if (themeSwitch.checked) {
            htmlElement.setAttribute("data-bs-theme", "dark");
            localStorage.setItem("theme", "dark");
        } else {
            htmlElement.setAttribute("data-bs-theme", "light");
            localStorage.setItem("theme", "light");
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("home-toggle").addEventListener("click", function () {
        window.location.href = homeUrl;
    });
});

document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-tooltip="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});

document.addEventListener("input", function (event) {
    if (event.target.classList.contains("auto-expand")) {
        event.target.style.height = "auto"; 
        event.target.style.height = event.target.scrollHeight + "px"; 
    }
});

document.addEventListener("DOMContentLoaded", function () {
    if (!window.location.hash) {
        window.location.hash = "#generalTemplates";
    }
});

document.getElementById("ticketForm").addEventListener("submit", function (event) {
    event.preventDefault(); 

    const summary = document.getElementById("summary").value;
    const priority = document.getElementById("priority").value;
    const pageUrl = window.location.href;

    if (!summary || !priority) {
        alert("The title and priority must be entered.");
        return;
    }

    fetch("/Jira/Create", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            summary: summary,
            priority: priority,
            pageUrl: pageUrl
        })
    })
        .then(response => {
            if (response.ok) {
                bootstrap.Modal.getInstance(document.getElementById('ticketModal')).hide();
            } else {
                alert("An error occurred. Please try again.");
            }
        })
        .catch(error => console.error("Error when sending a request:", error));
});