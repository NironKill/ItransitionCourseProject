document.getElementById("confirm-create").addEventListener("click", function () {
    document.getElementById("hidden-template-title").value = document.getElementById("template-title").innerText.trim();
    document.getElementById("hidden-template-description").value = document.getElementById("template-description").innerText.trim();
    document.getElementById("hidden-template-tag").value = document.getElementById("template-tag").innerText.trim();

    const isPublic = document.getElementById("publicAccess").checked;
    document.getElementById("create-template-form").insertAdjacentHTML("beforeend",
        `<input type="hidden" name="IsPublic" value="${isPublic}" />`);

    const questions = [];
    
    document.querySelectorAll("#questions-container .list-group-item").forEach((questionElement, questionIndex) => {
        const questionText = questionElement.querySelector("textarea").value.trim();
        const questionType = questionElement.querySelector("select").value;
        let questionId = questionElement.getAttribute("id");

        const guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
        if (!guidRegex.test(questionId)) {
            questionId = "00000000-0000-0000-0000-000000000000";
        }

        const options = [];
        questionElement.querySelectorAll(".option-item input[type='text']").forEach((optionElement, optionIndex) => {
            const optionId = optionElement.closest(".option-item").getAttribute("id")?.replace("option-", "") || "00000000-0000-0000-0000-000000000000";
            options.push({
                Id: optionId,
                Description: optionElement.value.trim(),
                Order: optionIndex
            });
        });

        questions.push({
            Id: questionId,
            Description: questionText,
            Type: questionType,
            Options: options,
            Order: questionIndex
        });
    });

    document.getElementById("create-template-form").insertAdjacentHTML("beforeend",
        `<input type="hidden" name="Questions" value='${JSON.stringify(questions)}' />`);

    document.getElementById("create-template-form").submit();
});

let questionCount = 0;

function addQuestion() {
    questionCount++;
    const questionId = `question-${questionCount}`;

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
                <textarea id="question-description-id" class="form-control auto-expand" placeholder="${localizedStrings.writeQuestion}" rows="1"></textarea>
        
                <select id="question-type-id" class="form-select question-type-selector">
                    <option value="1">${localizedStrings.checkbox}</option>
                    <option value="2">${localizedStrings.singleline}</option>
                    <option value="3">${localizedStrings.multiLine}</option>
                    <option value="4">${localizedStrings.integer}</option>
                </select>
            </div>
            
            <div id="options-${questionId}" class="mb-3">
                <div class="d-flex align-items-center mb-2 option-item">
                    <span class="btn-selector" style="cursor: grab;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
                            <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
                        </svg>
                        <i class="bi bi-three-dots-vertical"></i>
                    </span>
                    <input id="option-title-id" type="text" class="form-control" placeholder="${localizedStrings.option} 1">
                    <span class="btn btn-outline-secondary btn-sm ms-2" onclick="removeOption(this)">
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-lg" viewBox="0 0 16 16">
                            <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z" />
                        </svg>
                        <i class="bi bi-x-lg"></i>
                    </span>
                </div>
                <div class="d-flex align-items-center mb-2">
                    <button class="btn btn-link text-secondary p-0" onclick="addOption('${questionId}')">${localizedStrings.addOption}</button>
                </div>
            </div>
            <div class="d-flex align-items-center mb-2">
                <button class="btn btn-danger btn-sm ms-2" onclick="removeQuestion('${questionId}')">${localizedStrings.removeQuestion}</button>
            </div>
        </div>
    </div>
    `;

    document.getElementById("questions-container").insertAdjacentHTML('beforeend', questionHTML);

    updateSortable();
}

function addOption(questionId) {
    const optionsContainer = document.getElementById(`options-${questionId}`).querySelector("div");

    const optionIndex = optionsContainer.querySelectorAll(".option-item").length;

    const optionHTML = `
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
    optionsContainer.insertAdjacentHTML("beforebegin", optionHTML);
    updateOptionNumbers(questionId);
}

function removeQuestion(questionId) {
    document.getElementById(questionId).remove();
}

function removeOption(button) {
    const optionContainer = button.closest(".list-group");
    button.parentElement.remove();
    updateOptionNumbers(optionContainer.querySelector("[id^=options-]").id);
}

function updateOptionNumbers(optionsContainerId) {
    const optionsContainer = document.getElementById(optionsContainerId);
    optionsContainer.querySelectorAll(".option-item").forEach((option, index) => {
        option.querySelector("input[type='text']").placeholder = `${localizedStrings.option} ${index + 1}`;
    });
}

function updateSortable() {
    new Sortable(document.getElementById("questions-container"), {
        animation: 150,
        handle: ".btn-selector-quest",
    });

    document.querySelectorAll("[id^=options-]").forEach(optionList => {
        new Sortable(optionList, {
            animation: 150,
            handle: ".btn-selector",
            onEnd: () => updateOptionNumbers(optionList.id)
        });
    });
}

document.getElementById("questions-container").addEventListener("change", function (event) {
    if (event.target.classList.contains("question-type-selector")) {
        const questionCard = event.target.closest(".list-group-item");
        const optionsContainer = questionCard.querySelector(`[id^="options-"]`);

        const selectedType = event.target.value;
        optionsContainer.innerHTML = "";

        const typeHandlers = {
            2: () => createPlaceholder(optionsContainer, localizedStrings.placeholderSingleline),
            3: () => createPlaceholder(optionsContainer, localizedStrings.placeholderMultiline),
            4: () => createPlaceholder(optionsContainer, localizedStrings.placeholderInteger),
            1: () => createCheckboxOptions(optionsContainer),
        };

        typeHandlers[selectedType]?.();
    }
});

function createPlaceholder(container, placeholder) {
    const optionHTML = `
    <div class="d-flex align-items-center mb-2">
        <span class="form-text text-muted">${placeholder}</span>
    </div>
    `;
    container.insertAdjacentHTML("beforeend", optionHTML);
}

function createCheckboxOptions(container) {
    const optionHTML = `
    <div id="options-${container.id}" class="mb-3">
        <div class="d-flex align-items-center mb-2 option-item">
            <span class="btn-selector" style="cursor: grab;">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots-vertical" viewBox="0 0 16 16">
                    <path d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
                </svg>
                <i class="bi bi-three-dots-vertical"></i>
            </span>
            <input type="text" class="form-control" placeholder="${localizedStrings.option} 1">
            <span class="btn btn-outline-secondary btn-sm ms-2" onclick="removeOption(this)">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-lg" viewBox="0 0 16 16">
                    <path d="M2.146 2.854a.5.5 0 1 1 .708-.708L8 7.293l5.146-5.147a.5.5 0 0 1 .708.708L8.707 8l5.147 5.146a.5.5 0 0 1-.708.708L8 8.707l-5.146 5.147a.5.5 0 0 1-.708-.708L7.293 8z" />
                </svg>
                <i class="bi bi-x-lg"></i>
            </span>
        </div>
        <div class="d-flex align-items-center mb-2">
            <button class="btn btn-link text-secondary p-0" onclick="addOption('${container.id}')">${localizedStrings.addOption}</button>
        </div>
    </div>
    `;

    container.insertAdjacentHTML("beforeend", optionHTML);
    updateSortable();
}

document.getElementById("clean-template-toggle").addEventListener("click", function () {
    const questionsContainer = document.getElementById("questions-container");

    questionsContainer.innerHTML = "";

    document.querySelectorAll("input[name='Questions']").forEach(input => input.remove());
});