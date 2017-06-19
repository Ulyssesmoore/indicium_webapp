// e-mail suggestion
window.onload = function () {
    var firstname = document.getElementById("FirstName"),
        lastname = document.getElementById("LastName"),
        email = document.getElementById("Email"),
        suggestedEmail = document.getElementById("suggestedemail");
    
    function getEmail() {
        var result = firstname.value + '.' + lastname.value + '@student.hu.nl';
        return result.replace(/\s/g, '').toLowerCase();
    }
    
    firstname.addEventListener('input', function() { generateEmail() });
    lastname.addEventListener('input', function() { generateEmail() });
    email.addEventListener('input', function() { generateEmail() });
    
    suggestedEmail.onclick = function() {
        var result = getEmail();
        
        email.value = result;

        if (email.value == result) {
            suggestedEmail.innerHTML = '';
        }
    };

    function generateEmail() {
        var result = getEmail();

        if (firstname.value != '' && lastname.value != '') {
            if (email.value != result) {
                suggestedEmail.innerHTML = 'Voorgestelde email: ' + result;
            } else {
                suggestedEmail.innerHTML = '';
            }
        } else {
            suggestedEmail.innerHTML = '';
        }
    }
};