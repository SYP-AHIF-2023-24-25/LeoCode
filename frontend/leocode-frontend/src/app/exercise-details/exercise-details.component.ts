import { Component, OnInit } from '@angular/core';
import { ExerciseDto } from '../model/exerciseDto';
import { RestService } from '../service/rest.service';
import { DbService } from '../service/db-service.service';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-exercise-details',
  templateUrl: './exercise-details.component.html',
  styleUrls: ['./exercise-details.component.css']
})
export class ExerciseDetailsComponent implements OnInit {

  exerciseName: string|null = ""; 
  userName: string |null = "";
  user:string = "hans"

  exercise : ExerciseDto = {
    name: "",
    creator: "",
    description: "",
    language: "",
    tags: [],
    arrayOfSnippets: [],
    dateCreated: new Date(),
    dateUpdated: new Date()
  }

  constructor(private rest: RestService,private route: ActivatedRoute, private restDb: DbService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params: Params) => {
      this.userName = params['userName'];
      this.exerciseName = params['exerciseName'];

      if(this.userName != null && this.exerciseName != null){
        sessionStorage.setItem("userName", this.userName);
        sessionStorage.setItem("exerciseName", this.exerciseName);
      }else{
        this.userName = sessionStorage.getItem('userName');
        this.exerciseName = sessionStorage.getItem('exerciseName');
      }
    });

     if(this.userName != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.userName, this.exerciseName).subscribe((data: ExerciseDto[]) => {
        this.exercise = data[0];
        console.log(this.exercise.tags.length);
        console.log(this.exercise.creator);
        console.log(this.exercise.dateCreated);
        console.log(this.userName);

    });
    }
  }



  SaveChanges(){
    this.exercise.dateUpdated = new Date;
    console.log(this.exercise)

    //this.restDb.UpdateExercise(this.exercise.creator,this.exercise.description,this.exercise.language,this.exercise.tags,this.exercise.name,this.exercise.arrayOfSnippets)
   
  }
}
