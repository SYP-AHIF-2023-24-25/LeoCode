import { Component, OnInit } from '@angular/core';
import { ExerciseDto } from '../model/exerciseDto';
import { RestService } from '../service/rest.service';
import { DbService } from '../service/db-service.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';
import {Tags} from '../model/tags.enum';
import { map, Observable, startWith } from 'rxjs';
import { forEach } from 'jszip';

@Component({
  selector: 'app-exercise-details',
  templateUrl: './exercise-details.component.html',
  styleUrls: ['./exercise-details.component.css']
})
export class ExerciseDetailsComponent implements OnInit {
  ifUserName: string | null = '';
  creator: string | undefined = '';
  exerciseName: string|null = ""; 

  tagCtrl = new FormControl();
  availableTags: string[] = Object.values(Tags); // ersetzen Sie dies durch Ihre tatsächlichen Tags
  filteredTags: Observable<string[]> | undefined;  
  separatorKeysCodes: number[] = [13, 188]; // Enter und Komma  

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

  constructor(private rest: RestService,private route: ActivatedRoute, private restDb: DbService, private router: Router) {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice()));  
  }

  async logout(): Promise<void> {
    sessionStorage.setItem('shouldLogOut', 'true');
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    this.ifUserName = sessionStorage.getItem('ifUserName');
    this.route.queryParams.subscribe((params: Params) => {
      this.exerciseName = params['exerciseName'];
      this.creator = params['creator'];

      if(this.ifUserName != null && this.exerciseName != null){
        sessionStorage.setItem("exerciseName", this.exerciseName);
      }else{
        this.exerciseName = sessionStorage.getItem('exerciseName');
      }
    });

     if(this.ifUserName != null && this.exerciseName != null){
      //todo: den creator dieser task abfragen
      this.restDb.getExerciseByUsername(this.creator, this.exerciseName).subscribe((data: ExerciseDto[]) => {
        console.log("response von db für data:" + data);
        this.exercise = data[0];
        console.log(this.exercise.tags.length);
        console.log(this.exercise.creator);
        console.log(this.exercise.dateCreated);
        console.log(this.ifUserName);
        console.log(this.exercise.tags);
    });
    }

  }


  // tags auwählen
  toggleTagSelection(tag: string) {
    if (this.exercise.tags.includes(tag)) {
      this.exercise.tags = this.exercise.tags.filter(t => t !== tag);
    } else {
      this.exercise.tags.push(tag);
    }
  }

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our tag
    if ((value || '').trim()) {
      this.exercise.tags.push(value.trim().toUpperCase());
      this.availableTags.push(value.trim().toUpperCase());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }

    this.tagCtrl.setValue(null);
  }


  isSelectedTag(tag: string): boolean {
    return this.exercise.tags.includes(tag);
  }

  removeTag(tag: string): void {
    const index = this.exercise.tags.indexOf(tag);

    if (index >= 0) {
      this.exercise.tags.splice(index, 1);
    }
  }


  selected(event: MatAutocompleteSelectedEvent): void {
    this.exercise.tags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }



  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }
  

  SaveChanges(){
    this.exercise.dateUpdated = new Date;
    console.log(this.exercise)

    //this.restDb.UpdateExercise(this.exercise.creator,this.exercise.description,this.exercise.language,this.exercise.tags,this.exercise.name,this.exercise.arrayOfSnippets)
   
  }
}
