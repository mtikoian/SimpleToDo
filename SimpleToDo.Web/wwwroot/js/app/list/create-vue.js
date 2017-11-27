﻿(function (Vue, VeeValidate) {
    Vue.use(VeeValidate);

    var createList = new Vue({
        el: '#create-list',
        data: {
            list: {
                Name: ''
            },
            errorMessage: ''
        },
        methods: {
            onSubmit: function () {                
                var list = this.list;

                $.ajax({
                    url: window.location.origin + '/api/ListsApi',
                    data: JSON.stringify(list),
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function (response) {
                        window.location.href = response.redirect;
                    },
                    error: function (request, error) {

                    }
                });
            }
        }
    });
})(Vue, VeeValidate);