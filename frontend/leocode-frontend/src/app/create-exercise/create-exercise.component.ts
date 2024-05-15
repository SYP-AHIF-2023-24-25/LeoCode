import { Component, OnInit } from '@angular/core';
import {Tags} from '../model/tags.enum';
import { Exercise } from '../model/exercise';
import * as JSZip from 'jszip';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject } from '@angular/core';
import { RestService } from '../service/rest.service';
import { FileUploadService } from '../service/file-upload-service.service';
import { ElementCompact, xml2js } from 'xml-js';


@Component({
  selector: 'app-create-exercise',
  templateUrl: './create-exercise.component.html',
  styleUrls: ['./create-exercise.component.css']
})
export class CreateExerciseComponent {

  constructor(private fileUploadService: FileUploadService, private rest: RestService, private http: HttpClient) { }

  

  currentStep: number = 1;
  instruction: string = '';
  selectedLanguage: string = '';
  selectedTags: string[] = [];
  zipFile: File | null = null;
  emptyZipFile: File | null = null;

  searchTag: string = '';

  exercises : Exercise[]= [
    {
      instruction: 'Schreibe eine Funktion, die die Summe von zwei Zahlen berechnet.',
      language: 'Typescript',
      tags: ['POSE', 'TYPESCRIPT'],
      zipFile: null,
      emptyZipFile: null
    },
    {
      instruction: 'Schreibe eine Funktion, die die Summe von zwei Zahlen berechnet.',
      language: 'Csharp',
      tags: ['WMC', 'CSHARP'],
      zipFile: null,
      emptyZipFile: null
    },
  ]

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

  uploadEmptyZipFile(event: any) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement && inputElement.files && inputElement.files.length > 0) {
      this.emptyZipFile = inputElement.files[0];
    } else {
      console.error('No file selected');
    }
  }

  uploadZipFile(event: any) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement && inputElement.files && inputElement.files.length > 0) {
      this.zipFile = inputElement.files[0];
    } else {
      console.error('No file selected');
    }
  }

  async uploadZipFileToCSharpRunner(file: File, content: string): Promise<any> {
    if (!file) {
      console.error('No file selected');
      return; // Return early if no file is selected
    }
    try {
      let result = await (await this.fileUploadService.uploadCSharpTemplate(file, content)).toPromise();
      console.log(result);
      return result;
    } catch (error) {
      console.error('Error occurred during file upload:', error);
      throw error; // Rethrow the error for handling in the calling function
    }
  }

  async uploadZipToTsRunner(file: File, content: string): Promise<void > {
    if (!file) {
      console.error('No file selected');
      return; // Return early if no file is selected
    }
  
    try {
      const response: any = await this.fileUploadService.uploadFile(file, content).toPromise();
      console.log(response);
      return response;
    } catch (error) {
      console.error('Error occurred during file upload:', error);
      throw error; // Rethrow the error for handling in the calling function
    }
  }
  

  async sendCodeToRunner() {
     let exercise  = {
      instruction: this.instruction,
      language: this.selectedLanguage,
      tags: this.selectedTags,
      zipFile: this.zipFile,
      emptyZipFile: this.emptyZipFile
    };

    this.exercises.push(exercise);
    this.resetForm();

    console.log(this.filteredExercises);

    if (exercise.zipFile) {
      if(exercise.language === 'Typescript'){
        const fullResponse = await this.uploadZipToTsRunner(exercise.zipFile, "full");
        if (exercise.emptyZipFile && this.testsMatchPasses(fullResponse)) {
          await this.uploadZipToTsRunner(exercise.emptyZipFile, "empty");
        }
      }
      else if(exercise.language === 'Csharp'){
        const fullResponse = await this.uploadZipFileToCSharpRunner(exercise.zipFile, "full");

        if (this.testsMatchPassesCSharp(fullResponse) && exercise.emptyZipFile) {
          await this.uploadZipFileToCSharpRunner(exercise.emptyZipFile, "empty");
        }
      }
      
    }
  }
  testsMatchPassesCSharp(response: any): boolean {
    const xmlString = response;
    const xmlObject = xml2js(xmlString, { compact: true }) as ElementCompact; // Add type assertion
    const totalTests = xmlObject['TestRun'].ResultSummary.Counters._attributes.total;
    const passedTests = xmlObject['TestRun'].ResultSummary.Counters._attributes.passed;
    return totalTests === passedTests;
  }

  testsMatchPasses(response: any): boolean {
    return response && response.tests && response.passes &&
           response.tests.length === response.passes.length;
  }

  sleep(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
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
    this.emptyZipFile = null;
  }

}


