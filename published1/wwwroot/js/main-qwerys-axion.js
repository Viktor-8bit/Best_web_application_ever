async function get_users() {
    axios.get('/api/users')
        .then(function (response) {
            //console.log(response.data);
            //console.log(response.status);
            //console.log(response.statusText);
            //console.log(response.headers);
            //console.log(response.config);

            UsersTable._data.uesrsList = response.data;
        });
}

async function post_user_data() {
    let name = document.getElementById('Name');
    let age = document.getElementById('Age');

    //alert(name.value + " " + age.value);
    axios({
        method: 'post',
        url: '/api/users',
        data: {
            age: age.value,
            name: name.value
        }
    }).then(function (response) {
        console.log(response.data);
        get_users();
    });
}

async function delete_user(id) {
    axios({
        method: 'delete',
        url: '/api/users' + '/' + id,
    }).then(function (response) {
        console.log(response.status)
        get_users()
    });
}

async function change_user(_user) {
    let name = document.getElementById('Canvas-Name').value;
    let age = document.getElementById('Canvas-Age').value;

    console.log(name, age)
    axios({
        method: 'put',
        url: '/api/users',
        data: {
            id: _user.id,
            age: age,
            name: name
        }
    }).then(function (response) {
        console.log(response.status)
        console.log(response.data)
        get_users()
    });
}


