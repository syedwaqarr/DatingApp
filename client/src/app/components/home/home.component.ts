import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  ngOnInit(): void {
    this.getUsers();
  }

  http = inject(HttpClient);
  registerMode = false;
  users : any;

  cancelRegisterMode(event:boolean){
    this.registerMode = event;
  }

  getUsers(){
    return this.http.get("http://localhost:5190/api/users").subscribe({
      next:(response) => {this.users=response},
      error:() => console.log(console.error),
      complete:() => {console.log('completed');
      }
      
    })
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

}
