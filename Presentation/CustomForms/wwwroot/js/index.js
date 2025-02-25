document.addEventListener("DOMContentLoaded", function () {
    const selectElement = document.getElementById("filter-section-home");
    const filterInput = document.getElementById("search-template");

    const userRole = document.getElementById("filter-section-home").dataset.userRole;
    if (userRole !== "Admin") {
        document.getElementById("admin-manage-option").remove();
    }

    function filterTemplates(section) {
        let filteredTemplates = [];

        if (!section) {
            section = "generalTemplates";
        }

        switch (section) {
            case "mytemplates":
                filteredTemplates = templates.filter(t => t.userId === currentUserId);
                break;
            case "myforms":
                filteredTemplates = [];
                break;
            case "adminmanage":
                filteredTemplates = templates.filter(t => t.userId !== currentUserId);
                break;
            case "generalTemplates" :
                filteredTemplates = templates.filter(t => t.isPublic && t.userId !== currentUserId);
                break;
        }

        populateTable(section, filteredTemplates);
    }

    function populateTable(section, data) {
        const tableBody = document.querySelector(`#${section} tbody`);
        if (!tableBody) return; 

        tableBody.innerHTML = "";

        data.forEach((template, index) => {
            let authorColumn = "";
            let accessColumn = "";

            if (section === "generalTemplates" || section === "myforms") {
                authorColumn = `<td class="template-author">${template.author}</td>`;
            }

            if (section === "adminmanage") {
                authorColumn = `<td class="template-author">${template.author}</td>`;
                accessColumn = `<td class="template-access">${template.isPublic ? localizedStrings.public : localizedStrings.private}</td>`;
            }

            if (section === "mytemplates") {
                accessColumn = `<td class="template-access">${template.isPublic ? localizedStrings.public : localizedStrings.private}</td>`;
            }

            let row =
            `<tr data-id="${template.id}" data-section="${section}" class="clickable-row template-row">
                <td class="template-index">${index + 1}</td>
                <td class="template-topic">${template.topic}</td>
                <td class="template-title">${template.title}</td>
                ${authorColumn}
                ${accessColumn}    
                <td class="template-likes" data-likes="${template.numberLikes}" data-liked="${template.isLiked}">
                    <span class="like-count">${template.numberLikes}</span> 
                </td>
            </tr>`;
            tableBody.innerHTML += row;
        });
        attachRowClickHandlers();
    }

    function attachRowClickHandlers() {
        document.querySelectorAll(".clickable-row").forEach(row => {
            row.addEventListener("click", function () {
                const templateId = this.getAttribute("data-id");
                const section = this.getAttribute("data-section");

                handleRowClick(section, templateId);
            });
        });

        document.querySelectorAll(".template-likes").forEach(likeCell => {
            likeCell.addEventListener("click", function (event) {
                event.stopPropagation();

                let tr = this.closest("tr");
                let templateId = tr.getAttribute("data-id");
                let currentLikes = parseInt(this.getAttribute("data-likes"));
                let isLiked = this.getAttribute("data-liked") === "true";
                let likeCountSpan = tr.querySelector(".like-count");

                let url = isLiked ? `/Like/Remove/${templateId}` : `/Like/Add/${templateId}`;
                let method = isLiked ? "DELETE" : "POST";

                fetch(url, {
                    method: method,
                    headers: { "Content-Type": "application/json" }
                })
                    .then(data => {              
                        let newLikes = isLiked ? Math.max(0, currentLikes - 1) : currentLikes + 1;
                        this.setAttribute("data-likes", newLikes);
                        this.setAttribute("data-liked", isLiked ? "false" : "true");
                        likeCountSpan.textContent = newLikes;         
                    })
            });
        });
    }

    function handleRowClick(section, templateId) {
        switch (section) {
            case "generalTemplates":
                window.location.href = `/Form/Fill/${templateId}`;
                break;
            case "mytemplates":
                window.location.href = `/Template/Edit/${templateId}`;
                break;
            case "myforms":
                window.location.href = `/Form/View/${templateId}`;
                break;
            case "adminmanage":
                window.location.href = `/Template/Edit/${templateId}`;
                break;
        }
    }

    function showSectionFromHash() {
        let hash = window.location.hash.substring(1) || "generalTemplates";
        document.querySelectorAll(".section-content").forEach(section => {
            section.style.display = section.id === hash ? "block" : "none";
        });
        selectElement.value = hash;
        filterTemplates(hash);
    }

    selectElement.addEventListener("change", function () {
        window.location.hash = this.value;
    });

    window.addEventListener("hashchange", showSectionFromHash);
    showSectionFromHash();

    filterInput.addEventListener("input", function () {
        const searchTerm = filterInput.value.toLowerCase();

        document.querySelectorAll(".template-table tbody .template-row").forEach(row => {
            const title = row.querySelector(".template-title").textContent.toLowerCase();
            const topic = row.querySelector(".template-topic").textContent.toLowerCase();

            if (title.includes(searchTerm) || topic.includes(searchTerm)) {
                row.style.display = "";
            } else {
                row.style.display = "none";
            }
        });
    });
});
