import { Component } from '@angular/core';
import {Tags} from '../model/tags.enum';



@Component({
  selector: 'app-create-exercise',
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})
export class CreateExerciseComponent {

  currentStep: number = 1;
  instructions: string = '';
  selectedLanguage: string = '';
  selectedTags: string[] = [];
  zipFile: File | null = null;

  // Hier die verfügbaren Tags aus dem Enum abrufen
  availableTags: string[] = Object.values(Tags);

  nextStep() {
    this.currentStep++;
  }

  goToStep(step: number) {
    this.currentStep = step;
  }


  addInstructions(instructions: string) {
    this.instructions = instructions;
    this.nextStep(); // Nächster Schritt
  }

  selectLanguage(language: string) {
    this.selectedLanguage = language;
    this.nextStep(); // Nächster Schritt
  }

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
    let exercise = {
      instructions: this.instructions,
      language: this.selectedLanguage,
      tags: this.selectedTags,
      zipFile: this.zipFile
    };

    console.log('Exercise:', exercise);
    
    // Hier den Code zum Senden des extrahierten Codes und anderer Informationen an das Backend implementieren
    console.log('Anleitung:', this.instructions);
    console.log('Sprache:', this.selectedLanguage);
    console.log('Tags:', this.selectedTags);
    console.log('Zip-Datei:', this.zipFile);
    // Hier kannst du den Code und andere Informationen an das Backend senden
  }
}
