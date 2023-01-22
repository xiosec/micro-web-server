import {Iinformation} from "./Iinformation.js"

const ADDRES='http://127.0.0.1:8080/';

class Main{
    url:string
    constructor(url:string){
        this.url = url
        var btnSearch: any = document.getElementById("btn-search")
        var txtSearch: any = document.getElementById("txt-search")
        btnSearch.onclick = (e: Event) => {
            this.search(txtSearch?.value)
        }
        var btnReload: any = document.getElementById("btn-reload")
        btnReload.onclick=(e:Event)=>{
            this.get().then((data)=>{
                this.show(data)
            })
        }
        var txtFirstname: any = document.getElementById("txt-first")
        var txtLastname: any = document.getElementById("txt-last")
        var txtAddress: any = document.getElementById("txt-address")
        var txtEmail: any = document.getElementById("txt-email")
        var btnCreate: any = document.getElementById("btn-create")
        btnCreate.onclick=(e:Event)=>{
            if(txtFirstname.value.length >1
            && txtLastname.value.length >1
            && txtAddress.value.length >1
            && txtEmail.value.length >1){
                const data:object={
                    Firstname:txtFirstname.value,
                    Lastname:txtLastname.value,
                    Address:txtAddress.value,
                    Email:txtEmail.value
                }
                this.create(data)
            }
        }
    }
    get(): Promise<any> {
        return fetch(this.url)
            .then(response => response.json())     
    }
    delete(id: number): any {
        fetch(`${ADDRES}delete?id=${id}`).then((value) => {
            value.json().then((data) => {
                if (data.status) {
                    document.getElementById(`row${id}`)?.remove()
                }
            })
        })
    }
    private add(data:Iinformation):void{
        var table: any = document.getElementById("tb_main");
        var len:number = table.rows.length
        var row = table.insertRow(len)
        row.setAttribute('id', `row${data.Id}`)
        row.insertCell(0).innerHTML = len;
        row.insertCell(1).innerHTML = data.Firstname;
        row.insertCell(2).innerHTML = data.Lastname;
        row.insertCell(3).innerHTML = data.Address;
        row.insertCell(4).innerHTML = data.Email;
        row.insertCell(5).innerHTML = data.time;
        var btn:HTMLElement = document.createElement('button');
        btn.textContent = 'Delete';
        btn.setAttribute('class','btn btn-danger')
        btn.onclick = (e) => {
            this.delete(data.Id)
        }
        row.insertCell(6).appendChild(btn);
    }
    clearTable():void{
        var table: any = document.getElementById("tb_main");
        //clear old item
        table.innerHTML=null;
        var row = table.insertRow(0)
        row.insertCell(0).innerHTML = "#"
        row.insertCell(1).innerHTML = "First"
        row.insertCell(2).innerHTML = "Last"
        row.insertCell(3).innerHTML = "Address"
        row.insertCell(4).innerHTML = "Email"
        row.insertCell(5).innerHTML = "Time"
        row.insertCell(6).innerHTML = "option"
    }
    show(data: Array<Iinformation>): void{
        this.clearTable()
        data.forEach((element:Iinformation) => {
            this.add(element)
        });
    }
    search(name: string): void {
        if(name.length>1){
            fetch(`${this.url}find?name=${name}`)
               .then(response =>response.json().then(data => this.show(data)))
        }
    }
    create(data:object):void{
        fetch(`${this.url}create`,{
            method:"POST",
            body:JSON.stringify(data)
        }).then((value:Response)=>{
            value.json().then((data)=>{
                 this.add(data)
            })
        })
    }
}

var main = new Main(ADDRES);
main.get().then((value)=>{
    main.show(value)
})
