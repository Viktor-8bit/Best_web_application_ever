

Vue.component('user-table', {
    // Компонент todo-item теперь принимает
    // "prop", то есть входной параметр.
    // Имя входного параметра todo.
    props: ['user_list'],
    template: `
    <div class="container">
        <div class="row justify-content-md-center">
             <table class="table">
                    <thead class="table-dark">
                        <tr>
                            <th scope="col">Id</th>
                            <th scope="col">Имя</th>
                            <th scope="col">Возраст</th>
                            <th scope="col">Управление</th>

                        </tr>
                    </thead>
            
                    <tbody>
               
                       <tr v-for="user in user_list" 
                           v-bind:user="user"
                           v-bind:key="user.Id"  
                           id="hover">
                   
                           <th scope="row">{{ user.id }}</th>
                           <td>{{ user.name }}</td>
                           <td>{{ user.age }}</td>
                           <td colspan="1">
                                <button v-on:click="click_on_elemet( user )" type="button" class="btn btn-primary">Подробнее</button>
                           </td>

                       </tr>

                     </tbody>
                </table>
            </div>
        </div>
              `,
    methods: {
        click_on_elemet(_user) {
            //alert(id);
            //console.log(_user)
            CanvasPromt._data.viewUserId = _user;
            CanvasPromt._data.viewCanvas = true;
        }
    }
})

Vue.component('user-change-canvas', {
    props: ['my_user', 'is_view'],
    template: `
                <div v-show="is_view" >
                    <div class="modal:focus">
                    
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Информация о пользователе</h5>
                                    <button type="button" class="btn-close" v-on:click="clancel()" aria-label="Закрыть"></button>
                                </div>
                                <div class="modal-body">

                                    <div class="p-5">
                                        <form>

                                             <div class="form-group">
                                                <label>Id</label>
                                                <input type="number" class="form-control" id="Canvas-Id" :value="my_user.id" readonly>
                                            </div>

                                            <div class="form-group">
                                                <label>Имя пользователя</label>
                                                <input type="text" class="form-control" id="Canvas-Name" placeholder="Enter your name" :value="my_user.name">
                                            </div>

                                            <div class="form-group mb-3">
                                                <label>Возраст</label>
                                                <input type="number" class="form-control" id="Canvas-Age" placeholder="Your age" :value="my_user.age">
                                            </div>

                                        </form>
                                    </div>

                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger"  v-on:click="deleteUser(my_user)" >Удалить</button>
                                    <button type="button" class="btn btn-primary" v-on:click="changeUser(my_user)" >Сохранить изменения</button>
                                    <button type="button" class="btn btn-secondary"  v-on:click="clancel()" >Отменить</button>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
              `,
    methods: {
        save() {

        },
        clancel() {
            CanvasPromt._data.viewCanvas = false;
        },
        deleteUser(_user) {
            //console.log(_user)
            delete_user(_user.id);
            CanvasPromt._data.viewCanvas = false;
        },
        changeUser(_user) {

            change_user(_user);
            CanvasPromt._data.viewCanvas = false;
        }
    }
})

var UsersTable = new Vue({
    el: '#my_user-table',
    data: {
        uesrsList: [
            { id: 0, name: 'Амогус', age: 100 }
        ],
    }
});

var CanvasPromt = new Vue({
    el: '#usr-canvas',
    data: {
        viewCanvas: false,
        viewUserId: {}
    }
})



