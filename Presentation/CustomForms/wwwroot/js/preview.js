﻿$(document).ready(function () {

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
                <div class="d-flex justify-content-between align-items-center mb-3 gap-5">
                    <span class="form-control-plaintext" rows="1">${q.Description ?? ""}</span>
                </div>
                <div id="options-container-${q.Id}" class="options-container"></div>
                <div id="reply-box-${q.Id}" class="d-flex align-items-center mb-2"></div>
            </div>
        </div>`;

        $("#questions-container").append(questionHTML);

        const replyBoxHtml = getReplyBoxHtml(q);
        $(`#reply-box-${q.Id}`).html(replyBoxHtml);

        if (q.Type === 1 && q.Options && q.Options.length > 0) {
            q.Options.forEach((opt, optIndex) => {
                addExistingOption(q.Id, opt, optIndex);
            });
        }
    }

    function addExistingOption(questionId, opt) {
        const optionHtml = `
        <div class="form-check">
            <input class="form-check-input" type="checkbox" id="option-${opt.Id}" name="question-${questionId}" disabled>
            <label class="form-check-label" for="option-${opt.Id}">
                ${opt.Description ?? ""}
            </label>
        </div>`;

        $(`#options-container-${questionId}`).append(optionHtml);
    }

    function getReplyBoxHtml(question) {
        let staticHtml = "";

        switch (question.Type) {
            case 2: 
                staticHtml = `<p class="form-control-plaintext text-muted">${localizedStrings.placeholderSingleline}</p>`;
                break;
            case 3: 
                staticHtml = `<p class="form-control-plaintext text-muted">${localizedStrings.placeholderMultiline}</p>`;
                break;
            case 4: 
                staticHtml = `<p class="form-control-plaintext text-muted">${localizedStrings.placeholderInteger}</p>`;
                break;
        }

        return staticHtml;
    }

    document.getElementById('back-toggle').addEventListener('click', function () {
        window.location.href = `/Template/Edit/${model.TemplateId}`;
    });
});