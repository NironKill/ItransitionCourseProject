document.addEventListener("DOMContentLoaded", function () {
    if (localStorage.getItem("authSuccess")) {
        localStorage.removeItem("authSuccess"); 
        window.location.href = "/"; 
    }
});

document.querySelectorAll('.external-login-btn').forEach(button => {
    button.addEventListener('click', function () {
        var provider = this.getAttribute('data-provider');
        var url = this.getAttribute('data-url');

        var width = 500;
        var height = 600;
        var left = (window.innerWidth / 2) - (width / 2);
        var top = (window.innerHeight / 2) - (height / 2);

        var popup = window.open(url, provider, `width=${width},height=${height},top=${top},left=${left},resizable=yes`);

        var checkPopup = setInterval(function () {
            if (popup.closed) {
                clearInterval(checkPopup);
                location.reload(); 
            }
        }, 500); 
    });
});