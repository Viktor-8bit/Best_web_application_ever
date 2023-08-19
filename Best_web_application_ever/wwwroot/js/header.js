Vue.component('my-header', {
    props: ['user'],
    template: `
<nav class="navbar navbar-expand-lg navbar-light mynavbar">
  <div class="container-fluid">
    <a class="navbar-brand" href="#"><h5 class="mytext">Дешевые амогусы</h5></a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarSupportedContent">
      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
        <li class="nav-item">
          <a class="nav-link active mytext" aria-current="page" href="/"><h5 class="mytext">Главная</h5></a>
        </li>


        <li class="nav-item">
                <li v-if="user == undefined"><a class="nav-link active" href="/register"><h5 class="mytext">Регистрация</h5></a></li>
                <li v-if="user == undefined"><a class="nav-link active" href="/login"><h5 class="mytext">Вход</h5></a></li>
        </li>
          
        <li class="nav-item" >
                <li v-if="user != undefined"><a href="/logout" class="nav-link active"><h5 class="mytext">Выход</h5></a></li>
                <li v-if="user != undefined"><a class="nav-link active"><h5 class="mytext">{{ user }}</h5></a></li>
        </li>

      </ul>

    </div>
  </div>
</nav>
              `,
    methods: {

    }
})


var my_user_info = new Vue({
    el: '#my_usr_info',
    data: {
        user_name: getCookie('user_name'),
    }
})

    //< a class="navbar-brand" href = "/" > <h5 class="mytext">Главная</h5></a >

    //    <ul class="nav">

    //        <li class="nav-item">
    //            <a class="nav-link active" href="/register"><h5 class="mytext">Регистрация</h5></a>
    //        </li>

    //        <div v-if="user == undefined">
    //            <li class="nav-item">
    //                <a class="nav-link active" href="/login"><h5 class="mytext">Вход</h5></a>
    //            </li>
    //        </div>

    //        <div v-else>
    //            <li class="nav-item">
    //                <a class="nav-link active" href="/login"><h5 class="mytext">{{ user }}</h5></a>
    //            </li>
    //            <li class="nav-item">
    //                <a class="nav-link active" href="/logout"><h5 class="mytext">Выход</h5></a>
    //            </li>
    //        </div>

    //    </ul>

//getCookie('user_name');


