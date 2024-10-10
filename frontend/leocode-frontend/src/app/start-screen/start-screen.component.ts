import { Component } from '@angular/core';
import {Tags} from '../model/tags.enum';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { startWith } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { DbService } from '../service/db-service.service';
import { User } from '../model/user';
import { ExerciseDto } from '../model/exerciseDto';

@Component({
  selector: 'app-start-screen',
  templateUrl: './start-screen.component.html',
  styleUrls: ['./start-screen.component.css']
})
export class StartScreenComponent {


  constructor( private rest: DbService) {
    this.filteredTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice()));  

    
    this.filteredSearchTags = this.tagCtrl.valueChanges.pipe(
      startWith(null),
      map((tag: string | null) => tag ? this._filter(tag) : this.availableTags.slice())); 
   }

   defaultUser :User = {
    username: "Default",
    password: "Default",
    exercises: []
  }


  //für Suche in der Liste
  selectedSearchTags: string[] = [];
  filteredSearchTags: Observable<string[]> | undefined;

  // property für die ausgewählten Tags
  tagCtrl = new FormControl();
  selectedTags: string[] = [];
  availableTags: string[] = Object.values(Tags); // ersetzen Sie dies durch Ihre tatsächlichen Tags
  filteredTags: Observable<string[]> | undefined;
  separatorKeysCodes: number[] = [13, 188]; // Enter und Komma

  exercises : ExerciseDto[]= [];
  

  ngOnInit(): void {
    this.rest.getExerciseByUsername(this.defaultUser.username).subscribe((data: ExerciseDto[]) => {
      this.exercises = data;
      console.log(this.exercises);
    });

    sessionStorage.setItem("userName", this.defaultUser.username);

    document.addEventListener("DOMContentLoaded", () => {
      const mainMenuLinks = document.querySelectorAll('.main-menu-link');
      
      mainMenuLinks.forEach(link => {
        link.addEventListener('click', (event) => {
          event.preventDefault();
          const subMenu = link.nextElementSibling;
          if (subMenu) {
            subMenu.classList.toggle('show');
          }
        });
      });
    });
  }

  

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.availableTags.filter(tag => tag.toLowerCase().includes(filterValue));
  }

  removeSearchTag(tag: string): void {
    const index = this.selectedSearchTags.indexOf(tag);

    if (index >= 0) {
      this.selectedSearchTags.splice(index, 1);
    }
  }
  selectedSearch(event: MatAutocompleteSelectedEvent): void {
    this.selectedSearchTags.push(event.option.viewValue);
    this.tagCtrl.setValue(null);
  }

    // Getter-Funktion für gefilterte Übungen basierend auf dem Suchbegriff für Tags
    get filteredExercises() {
      if (!this.selectedSearchTags.length) {
        return this.exercises;
      }
    
      return this.exercises.filter(exercise =>
        exercise.tags.some(tag => this.selectedSearchTags.includes(tag))
      );
    }

    addExerciseNameToSessionStorage(exerciseName: string){
      sessionStorage.setItem("exerciseName", exerciseName);
    }
}
