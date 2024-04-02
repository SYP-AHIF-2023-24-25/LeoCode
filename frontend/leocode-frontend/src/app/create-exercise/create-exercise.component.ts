import { Component, OnInit } from '@angular/core';
import {Tags} from '../model/tags.enum';
import { Exercise } from '../model/exercise';
import { RestService } from '../service/rest.service';




@Component({
  selector: 'app-create-exercise',
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})
export class CreateExerciseComponent {

  currentStep: number = 1;
  instruction: string = '';
  selectedLanguage: string = '';
  selectedTags: string[] = [];
  zipFile: File | null = null;

  searchTag: string = '';

  exercises : Exercise[]= [
    {
      instruction: 'Schreibe eine Funktion, die die Summe von zwei Zahlen berechnet.',
      language: 'Typescript',
      tags: ['POSE', 'TYPESCRIPT'],
      zipFile: null
    },
    {
      instruction: 'Schreibe eine Funktion, die die Summe von zwei Zahlen berechnet.',
      language: 'Csharp',
      tags: ['WMC', 'CSHARP'],
      zipFile: null
    },
  ]

  constructor(private rest: RestService) {
  }

  // Hier die verfügbaren Tags aus dem Enum abrufen
  availableTags: string[] = Object.values(Tags);

  ngOnInit(): void {
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

  // Getter-Funktion für gefilterte Übungen basierend auf dem Suchbegriff für Tags
  get filteredExercises(): Exercise[] {
    if (!this.searchTag) {
      return this.exercises;
    }else{
      return this.exercises.filter(exercise =>
        exercise.tags.includes(this.searchTag.toUpperCase())
      );
    }
  }


  // navigaton zwischen den schritten
  nextStep() {
    this.currentStep++;
  }

  goToStep(step: number) {
    this.currentStep = step;
  }


  addInstructions(instructions: string) {
    this.instruction = instructions;
    this.nextStep(); 
  }

  selectLanguage(language: string) {
    this.selectedLanguage = language;
    this.nextStep(); 
  }

  // tags auwählen
  toggleTagSelection(tag: string) {
    if (this.selectedTags.includes(tag)) {
      this.selectedTags = this.selectedTags.filter(t => t !== tag);
    } else {
      this.selectedTags.push(tag);
    }
  }

  isSelectedTag(tag: string): boolean {
    return this.selectedTags.includes(tag);
  }

  uploadZipFile(event: any) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement && inputElement.files && inputElement.files.length > 0) {
      this.zipFile = inputElement.files[0];
    } else {
      console.error('No file selected');
    }
  }

  

  sendCodeToBackend() {
     let exercise  = {
      instruction: this.instruction,
      language: this.selectedLanguage,
      tags: this.selectedTags,
      zipFile: this.zipFile
    };

    this.exercises.push(exercise);

    console.log('Exercise:', exercise);
    console.log('____________________________')
    for(let i = 0; i < this.exercises.length; i++){
      console.log('Exercise:', this.exercises[i]);
    }
    this.resetForm();

    console.log(this.filteredExercises);
    console.log('____________________________');
    this.rest.uploadZipFile(exercise.zipFile, exercise.language).subscribe((data) => {
      console.log(data);
    },
    (error) => {
        console.error("Error in API request", error);
    });
  
  }

  runTest(exercise: Exercise) {
    console.log('Running test for exercise:', exercise);
  }

  resetForm() {
    this.currentStep = 1;
    this.instruction = '';
    this.selectedLanguage = '';
    this.selectedTags = [];
    this.zipFile = null;
  }

}
