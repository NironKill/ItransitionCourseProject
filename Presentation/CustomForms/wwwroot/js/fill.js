$(document).ready(function () {
    $("#btn-send-form").on("click", function () {
        let formData = {
            TemplateId: model.TemplateId,
            UserId: currentUserId,
            Answers: []
        };

        model.Questions.forEach(q => {
            let answer = {
                QuestionId: q.Id,
                Response: ""
            };

            if (q.Type === 1) { 
                let selectedOptions = [];
                $(`input[name="question-${q.Id}"]:checked`).each(function () {
                    let optionText = $(this).next("label").text().trim(); 
                    selectedOptions.push(optionText);
                });
                answer.Response = selectedOptions.join(" ");
            }
            else { 
                answer.Response = $(`#answer-${q.Id}`).val().trim();
            }

            formData.Answers.push(answer);
        });

        $.ajax({
            url: "/Form/Fill/",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(formData),
            success: function (response) {
                alert("Form submitted successfully!");
                window.location.href = "/Home/Index"; 
            },
            error: function (error) {
                console.error("Error submitting form:", error);
                alert("An error occurred while submitting the form.");
            }
        });
    });

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
       
        const replyBoxHtml = getReplyBoxHtml(q.Id, q.Type);
        $(`#reply-box-${q.Id}`).html(replyBoxHtml);

        if (q.Type === 1 && q.Options && q.Options.length > 0) {
            q.Options.forEach((opt, optIndex) => {
                addExistingOption(q.Id, opt, optIndex);
            });
        }
    }

    function addExistingOption(questionId, opt, optIndex) {
        const optionHtml = `
        <div class="form-check">
            <input class="form-check-input" type="checkbox" id="option-${opt.Id}" name="question-${questionId}">
            <label class="form-check-label" for="option-${opt.Id}">
                ${opt.Description ?? ""}
            </label>
        </div>`;

        $(`#options-container-${questionId}`).append(optionHtml);
    }

    function getReplyBoxHtml(questionId, questionType) {
        let inputHtml = "";

        switch (questionType) {
            case 2: 
                inputHtml = `
                <input type="text" class="form-control" id="answer-${questionId}" 
                    placeholder="${localizedStrings.placeholder}" maxlength="100">
                `;
                break;
            case 3: 
                inputHtml = `
                <textarea class="form-control" id="answer-${questionId}" 
                    placeholder="${localizedStrings.placeholder}" rows="4"></textarea>
                `;
                break;
            case 4: 
                inputHtml = `
                <input type="text" class="form-control" id="answer-${questionId}" 
                    placeholder="${localizedStrings.placeholder}" min="0" step="1"
                    oninput="this.value = this.value.replace(/[^0-9]/g, '');">
                `;
                break;
        }

        return inputHtml;
    } 
});