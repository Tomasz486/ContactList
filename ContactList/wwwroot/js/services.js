function contactController($scope, $http) {

    $scope.action = '';

    $scope.changeType = function () {
        $scope.contactForm.type = $scope.selectedType !== 'inny' ? $scope.selectedType : '';
    }

    $scope.getContactList = function () {
        $http.post("/Contact/ContactTypes").then(function (response) {
            $http.get("/Contact").then(function (responseContact) {
                $scope.contacts = responseContact.data;

                if (response.data) {
                    $scope.contactTypes = response.data;
                    $scope.selectedType = $scope.contactTypes[0];

                    if (!$scope.contactForm) {
                        $scope.contactForm = {};
                    }

                    $scope.contactForm.type = $scope.selectedType;
                }
                else {
                    $scope.message = response.data;
                }
            });
        });
    }

    $scope.newContact = function () {
        $scope.action = 'contactForm';

        if (!$scope.contactForm) {
            $scope.contactForm = {};
        }

        $scope.contactForm.id = 0;
    };

    $scope.saveContact = function () {
        $http.post($scope.action === "contactForm" ? "/Contact/Add" : "/Contact/Edit", {
            Id: $scope.contactForm.id, ContactName: $scope.contactForm.contactName,
            Surname: $scope.contactForm.surname, Email: $scope.contactForm.email,
            Phone: $scope.contactForm.phone, type: $scope.contactForm.type,
            DayOfBirth: $scope.contactForm.dayOfBirth, Token: $scope.token
        }).then(function (response) {
            if (response.data === "OK") {
                $scope.action = '';
                $scope.getContactList();
                $scope.message = "Pomyślnie zapisano zmiany";
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.editContact = function (contact) {
        contact.token = $scope.token;
        $http.post("/Contact/Select", contact).then(function (response) {
            if (response.data.id) {
                $scope.action = "EditContact";
                $scope.contactForm.id = response.data.id;
                $scope.contactForm.contactName = response.data.contactName;
                $scope.contactForm.surname = response.data.surname;
                $scope.contactForm.email = response.data.email;
                $scope.contactForm.phone = response.data.phone;
                $scope.contactForm.type = response.data.type;
                $scope.selectedType = $scope.contactTypes.includes($scope.contactForm.type) ? $scope.contactForm.type : 'inny';
                $scope.contactForm.dayOfBirth = response.data.dayOfBirth;
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.updateContact = function (contact) {
        contact.token = $scope.token;
        $http.post("/Contact/Edit", contact).then(function (response) {
            $scope.message = response.data;
        });
    }

    $scope.selectContact = function (contact) {
        contact.token = $scope.token;
        $http.post("/Contact/Select", contact).then(function (response) {

            if (response.data.id) {
                $scope.action = "SelectContact";
                $scope.selectedContact = angular.fromJson(response.data);
                $scope.message = "";
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.deleteContact = function (contact) {
        contact.token = $scope.token;
        if (confirm("Czy na pewno chcesz usunąć kontakt: " + contact.contactName) == true) {
            $http.post("/Contact/Delete", contact).then(function (response) {
                $scope.action = "";
                $scope.message = response.data;
                $scope.getContactList();
            });
        }
    }

    $scope.createUser = function () {
        $http.post("/User/Create", { Login: $scope.createUserForm.login, Password: $scope.createUserForm.password }).then(function (response) {
            if (response.data == "OK") {
                $scope.action = "";
                $scope.createUserForm = {};
                $scope.message = "Pomyślnie utworzono konto nowego użytkownika.";
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.logIn = function () {
        $http.post("/User/LogIn", { Login: $scope.loginForm.login, Password: $scope.loginForm.password }).then(function (response) {
            if (response.data.token) {
                $scope.token = response.data.token
                localStorage.setItem("token", $scope.token);
                $scope.action = "";
                $scope.loginForm = {};
                $scope.message = "Pomyślnie zalogowano użytkownika.";
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.logOut = function () {
        $http.post("/User/LogOut", JSON.stringify($scope.token)).then(function (response) {
            if (response.data == "OK") {
                $scope.token = "";
                $scope.selectedContact = {};
                $scope.contactForm = {};
                $scope.action = "";
                localStorage.clear();
                $scope.message = "Pomyślnie wylogowano użytkownika";
            }
            else {
                $scope.message = response.data;
            }
        });
    }

    $scope.checkToken = function () {
        $scope.token = localStorage.getItem("token");
        $http.post("/User/CheckToken", JSON.stringify($scope.token)).then(function (response) {
            if (response.data !== "OK") {
                $scope.token = '';
                localStorage.clear();
            }
        });
    };

    $scope.checkToken();

    $scope.validateEmail = function(email) {
        return String(email)
            .toLowerCase()
            .match(
                /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
            );
    };
}
