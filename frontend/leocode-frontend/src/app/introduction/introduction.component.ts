import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExerciseDto } from '../model/exerciseDto';
import { Params } from '@angular/router';
import { DbService } from '../service/db-service.service';

@Component({
  selector: 'app-introduction',
  templateUrl: './introduction.component.html',
  styleUrls: ['./introduction.component.css']
})
export class IntroductionComponent {

  userName: string|null = sessionStorage.getItem('userName');
  exerciseName: string | null = sessionStorage.getItem('exerciseName');
  date: Date = new Date();
  creator: string | undefined = "";

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
  constructor( private route: ActivatedRoute, private restDb: DbService) {
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params: Params) => {
      this.creator = params['creator'];   
    });
    
console.log(this.creator);
console.log(this.exerciseName);

    if(this.creator != null && this.exerciseName != null){
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: ExerciseDto[]) => {
        this.exercise = data[0];
       
        console.log(this.exercise.name);
        console.log(this.exercise.arrayOfSnippets);
        console.log(this.exercise.language);
        console.log(this.exercise.description);
    });
    }
  }
}
