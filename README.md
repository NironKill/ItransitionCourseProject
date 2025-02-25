You have to implement a Web application for customisable forms (quizzes, tests, questionnaires, polls, etc.). Something similar to Google Forms. Users define "templates" (the set of questions, their names and descriptions, etc.), and other users fill out "forms" (their specific answers) using these templates (e.g., enter or select values in the fields).
Non-authenticated users cannot create templates, leave comments and likes, or fill out the forms. But they can use search and view templates in read-only mode.

Admin-page allow user management—view; block; unblock; delete; add to admins; remove from admins. ADMIN IS ABLE TO REMOVE ADMIN ACCESS FROM ITSELF; it’s important.

Admin sees all pages as their author (for example, admin can open a template of another user and add a question to it or open a form for the user and edit an answer; so, admin is virtually the owner of every template and every form).

Filled-out form (answers) can be seen by the author who fill out this form as well as the creator of the responding template and admins. "Public" templates are accessible for viewing for everyone.

Only the admin or creator of the template can manage it (add/delete/edit questions).

Only the admin or creator of the form can manage it (delete it or edit answers).

Users can register and authenticate via site forms.

Every page (in the top header) provides access to the full-text search. Search results are always templates (e.g., if text is found in the question description or template comments, the search result links to the template itself, not to the question or comment).
