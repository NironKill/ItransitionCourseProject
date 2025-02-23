document.addEventListener("DOMContentLoaded", function () {
    const selectElement = document.getElementById("filter-section-home");

    const userRole = document.getElementById("filter-section-home").dataset.userRole;
    if (userRole !== "Admin") {
        document.getElementById("admin-manage-option").remove();
    }

    function showSectionFromHash() {
        let hash = window.location.hash.substring(1);

        document.querySelectorAll(".section-content").forEach(section => {
            section.style.display = section.id === hash ? "block" : "none";
        });

        selectElement.value = hash;
    }

    selectElement.addEventListener("change", function () {
        window.location.hash = this.value;
    });

    window.addEventListener("hashchange", showSectionFromHash);

    showSectionFromHash();
});