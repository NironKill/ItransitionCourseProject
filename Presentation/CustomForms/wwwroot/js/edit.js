$(document).ready(function () {
    if (model.Questions && model.Questions.length > 0) {
        model.Questions.forEach((q, index) => {
            addExistingQuestion(q, index);
        });
    }

    function addExistingQuestion(q, questionIndex) {
        const questionId = `${q.Id}`;

        const questionHTML = `
        <div id="questions-container" class="list-group">
            <div class="list-group-item card shadow-sm p-3 mb-3" id="${questionId}">
                <div class="d-flex justify-content-center mb-2">
                    <span class="btn-selector-quest" style="cursor: grab;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots" viewBox="0 0 16 16">
                            <path d="M3 9.5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3" />
                        </svg>
                        <i class="bi bi-three-dots"></i>
                    </span>
                </div>
                <div class="d-flex justify-content-between align-items-center mb-3 gap-5">
                    <textarea class="form-control auto-expand" placeholder="${localizedStrings.writeQuestion}" rows="1">${q.Description ?? ""}</textarea>
                    <select class="form-select question-type-select" data-question-index="${questionId}">
                        <option value="1" ${q.Type === 1 ? "selected" : ""}>${localizedStrings.checkbox}</option>
                        <option value="2" ${q.Type === 2 ? "selected" : ""}>${localizedStrings.singleline}</option>
                        <option value="3" ${q.Type === 3 ? "selected" : ""}>${localizedStrings.multiLine}</option>
                        <option value="4" ${q.Type === 4 ? "selected" : ""}>${localizedStrings.integer}</option>
                    </select>
                </div>
                <div id="options-container-${q.Id}" class="options-container"></div>
                <div id="sample-answer-${q.Id}" class="d-flex align-items-center mb-2"></div>
                <div class="d-flex align-items-center mb-2">
                    <button class="btn btn-danger btn-sm" onclick="removeQuestion('${questionId}')">${localizedStrings.removeQuestion}</button>
                </div>
            </div>
        </div>`;

        $("#questions-container").append(questionHTML);

        var placeholder = "def";
        switch (q.Type) {
            case 2:
                placeholder = localizedStrings.singleline;
                break;
            case 3:
                placeholder = localizedStrings.multiLine;
                break;
            case 4:
                placeholder = localizedStrings.integer;
                break;
        }

        const sampleAnswerHtml = getSampleAnswerHtml(q.Id, q.Type, placeholder);
        $(`#sample-answer-${q.Id}`).html(sampleAnswerHtml);

        if (q.Type === 1 && q.Options && q.Options.length > 0) {
            q.Options.forEach((opt, optIndex) => {
                addExistingOption(q.Id, opt, optIndex);
            });
        }
        updateSortable();
    }

    function addExistingOption(questionId, opt, optIndex) {
        const optionHtml = `
        <div class="d-flex align-items-center mb-2 option-item" id="option-${opt.Id}">
            <span class="btn-selector" style="cursor: grab;">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
                    <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
                </svg>
                <i class="bi bi-three-dots-vertical"></i>
            </span>
            <input type="text" class="form-control" placeholder="${localizedStrings.option} ${optIndex + 1}" value="${opt.Description ?? ""}" required>
            <span class="btn btn-outline-secondary btn-sm ms-2" onclick="removeOption(this)">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-lg" viewBox="0 0 16 16">
                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z" />
                </svg>
                <i class="bi bi-x-lg"></i>
            </span>
        </div>`;

        $(`#options-container-${questionId}`).append(optionHtml);
    }

    function getSampleAnswerHtml(questionId, questionType, placeholder) {
        if (questionType === 1) {
            return `<button type="button" class="btn btn-link text-secondary p-0 add-option" data-question-index="${questionId}">${localizedStrings.addOption}</button>`;
        }
        else {
            return `<span class="form-text text-muted">${placeholder}</span>`;
        }
    }

    $(document).on("click", ".add-option", function () {
        const questionIndex = $(this).data("question-index");
        const optionsContainer = $(`#options-container-${questionIndex}`);
        const optionIndex = optionsContainer.find(".option-item").length;

        const optionHtml = `
        <div class="d-flex align-items-center mb-2 option-item">
            <span class="btn-selector" style="cursor: grab;">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
                    <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
                </svg>
                <i class="bi bi-three-dots-vertical"></i>
            </span>
            <input type="text" class="form-control" placeholder="${localizedStrings.option} ${optionIndex + 1}">
            <span class="btn btn-outline-secondary btn-sm ms-2" onclick="removeOption(this)">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-lg" viewBox="0 0 16 16">
                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z" />
                </svg>
                <i class="bi bi-x-lg"></i>
            </span>
        </div>
        `;
        optionsContainer.append(optionHtml);
        updateOptionNumbers(questionIndex);
    });

    $(document).on("change", ".question-type-select", function () {
        const questionIndex = $(this).data("question-index");
        const questionType = parseInt($(this).val());
        var placeholder = "def";
        switch (questionType) {
            case 2:
                placeholder = localizedStrings.singleline;
                break;
            case 3:
                placeholder = localizedStrings.multiLine;
                break;
            case 4:
                placeholder = localizedStrings.integer;
                break;
        }
        $(`#options-container-${questionIndex}`).empty();
        const sampleAnswerContainer = $(`#sample-answer-${questionIndex}`);
        const sampleAnswerHtml = getSampleAnswerHtml(questionIndex, questionType, placeholder);
        sampleAnswerContainer.html(sampleAnswerHtml);
    });

    $(document).on("click", "#delete-toggle", async function () {
        const response = await fetch(`/Template/Delete/${model.Id}`, {
            method: "DELETE",
            headers: { "Content-Type": "application/json" }
        });

        if (response.ok) {
            window.location.href = "/";
        } else {
            console.error("Failed to delete template.");
            alert("An error occurred. Please try again.");
        }
    });

    $("#link-toggle").click(function () {
        const link = `${window.location.origin}/Form/Fill/${model.Id}`;

        navigator.clipboard.writeText(link)
            .then(() => {
                alert("This link has been copied to the clipboard!");
            })
    });

    document.getElementById('preview-toggle').addEventListener('click', function () {
        window.location.href = `/Template/Preview/${model.Id}`;
    });
});