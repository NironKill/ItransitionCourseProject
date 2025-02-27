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
                filteredTemplates = templates.filter(t => t.form !== null);
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
            `<tr data-id="${template.id}" data-section="${section}" form-id="${template.form ? template.form.id : ""}" class="clickable-row template-row">
                <td class="template-submenu">
                    <span class="submenu-toggle">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-right" viewBox="0 0 16 16">
                            <path d="M6 12.796V3.204L11.481 8zm.659.753 5.48-4.796a1 1 0 0 0 0-1.506L6.66 2.451C6.011 1.885 5 2.345 5 3.204v9.592a1 1 0 0 0 1.659.753"/>
                        </svg>
                        <i class="bi bi-caret-right"></i>
                    </span> 
                </td>
                <td class="template-index">${index + 1}</td>
                <td class="template-topic">${template.topic}</td>
                <td class="template-title">${template.title}</td>
                ${authorColumn}
                ${accessColumn}    
                <td class="template-likes" data-likes="${template.numberLikes}" data-liked="${template.isLiked}">
                    <span class="like-count">${template.numberLikes}</span> 
                </td>
            </tr>
            <tr class="submenu-row" data-id="${template.id}" data-section="${section}" style="display: none;">
                <td colspan="100%">
                    <div class="container submenu-content">
                        <div class="card p-3 shadow-sm">
                            <div class="row g-3">
                                <div class="col-md-9">
                                    <h5 class="fw-bold">${template.title}</h5>
                                    <p>${template.description}</p>
                                    <p class="text-muted template-tags"><strong>${localizedStrings.tags}:</strong> ${template.tags}</p>
                                    <input type="text"  id="comment-input" class="form-control" placeholder="${localizedStrings.addComment}" data-id="${template.id}">
                                    <p class="text-muted mb-1"><strong>${localizedStrings.comments}</strong></p>
                                    <div class="comment-list">
                                        ${template.comments.map(comment => `
                                            <div class="comment-item border-bottom py-2">
                                                <strong>${comment.userName}</strong> 
                                                <span class="text-muted small">
                                                    (${new Date(comment.commentedAt).toLocaleString()})
                                                </span>
                                                <p class="mb-0">${comment.content}</p>
                                            </div>
                                        `).join("")}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>`;
            tableBody.innerHTML += row;
        });
        attachSubmenuHandlers();
        attachCommentHandlers();
        attachRowClickHandlers();
    }
    function attachRowClickHandlers() {
        document.querySelectorAll(".clickable-row").forEach(row => {
            row.addEventListener("click", function () {
                const templateId = this.getAttribute("data-id");
                const formId = this.getAttribute("form-id");
                const section = this.getAttribute("data-section");

                handleRowClick(section, templateId, formId);
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

    function attachSubmenuHandlers() {
        document.querySelectorAll(".template-submenu").forEach(toggle => {
            toggle.addEventListener("click", function (event) {
                event.stopPropagation();

                let tr = this.closest("tr");
                let templateId = tr.getAttribute("data-id");
                let section = tr.getAttribute("data-section");
                let submenuRow = document.querySelector(`#${section} .submenu-row[data-id="${templateId}"]`);

                if (submenuRow.style.display === "none") {
                    submenuRow.style.display = "table-row";
                    this.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down" viewBox="0 0 16 16">
                                        <path d="M1.5 6.5L8 12l6.5-5.5z"/>
                                     </svg>`;
                } else {
                    submenuRow.style.display = "none";
                    this.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-right" viewBox="0 0 16 16">
                                         <path d="M6 12.796V3.204L11.481 8zm.659.753 5.48-4.796a1 1 0 0 0 0-1.506L6.66 2.451C6.011 1.885 5 2.345 5 3.204v9.592a1 1 0 0 0 1.659.753"/>
                                     </svg>`;
                }
            });
        });
    }

    function handleRowClick(section, templateId, formId) {
        switch (section) {
            case "generalTemplates":
                window.location.href = `/Form/Fill/${templateId}`;
                break;
            case "mytemplates":
                window.location.href = `/Template/Edit/${templateId}`;
                break;
            case "myforms":
                window.location.href = `/Form/View/${formId}`;
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
        const searchTerms = filterInput.value.toLowerCase().trim().split(/\s+/);

        document.querySelectorAll(".template-table tbody .template-row").forEach(row => {
            const title = row.querySelector(".template-title").textContent.toLowerCase();
            const topic = row.querySelector(".template-topic").textContent.toLowerCase();
            const tagElement = row.nextElementSibling?.querySelector(".template-tags");
            const tags = tagElement ? tagElement.textContent.toLowerCase().split(/\s+/) : [];;

            const matchesAllTerms = searchTerms.every(term =>
                title.includes(term) ||
                topic.includes(term) ||
                tags.some(tag => tag.includes(term))
            );

            row.style.display = matchesAllTerms ? "" : "none";
        });
    });

    function attachCommentHandlers() {
        document.querySelectorAll("#comment-input").forEach(input => {
            input.addEventListener("keypress", async function (event) {
                if (event.key === "Enter") {
                    event.preventDefault();
                    await submitComment(this);
                }
            });
        });
    }

    async function submitComment(inputElement) {
        const content = inputElement.value.trim();
        const templateId = inputElement.dataset.id;

        if (!content) return;

        try {
            const response = await fetch("/Comment/Create", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ templateId, content })
            });

            if (!response.ok) {
                console.error("Failed to submit comment");
                return;
            }

            const commentList = inputElement.closest(".submenu-content").querySelector(".comment-list");

            const newComment = {
                userName: localizedStrings.yourСomment, 
                commentedAt: new Date().toISOString(),
                content: content
            };

            commentList.innerHTML += `
                <div class="comment-item border-bottom py-2">
                    <strong>${newComment.userName}</strong> 
                    <span class="text-muted small">
                        (${new Date(newComment.commentedAt).toLocaleString()})
                    </span>
                    <p class="mb-0">${newComment.content}</p>
                </div>
            `;

            inputElement.value = "";
        } catch (error) {
            console.error("Error submitting comment:", error);
        }
    }
});
