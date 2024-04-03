"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
/*import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
import { runTs } from './execute-tests';
import fs from 'fs';
import swaggerUi from 'swagger-ui-express';
import { Snippets } from './model/snippets';
import path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';

const swaggerDocument = require('../swagger.json');


const app = express();
const multer = require('multer');
const port = 3000;

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerDocument));
app.use(cors());

app.use(bodyParser.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, Express!');
});

app.post('/api/execute/:exerciseName', async (req: Request, res: Response) => {
  const exerciseName = req.params.exerciseName;
  const fileName = req.body.fileName;
  const code = req.body.code;
  const templateFilePath = `./templates/${exerciseName}`;
  console.log(fileName);
  console.log(code);
  const result = await runTs(exerciseName, templateFilePath,code,fileName);
  res.status(200).json(result);
});

const storage = multer.diskStorage({
  destination: function (req:any, file:any, cb:any) {
    cb(null, './templates'); // Save uploaded files to the "uploads" directory
  },
  filename: function (req:any, file:any, cb:any) {
    cb(null, file.originalname);
  }

});

const upload = multer({ storage: storage });

// Upload route
app.post('/upload', upload.single('file'), (req, res) => {
  res.json({ message: 'File uploaded successfully' });
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});*/
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const cors_1 = __importDefault(require("cors"));
const execute_tests_1 = require("./execute-tests");
const fs_1 = __importDefault(require("fs"));
const swagger_ui_express_1 = __importDefault(require("swagger-ui-express"));
const path_1 = __importDefault(require("path"));
const multer_1 = __importDefault(require("multer"));
const unzipper_1 = __importDefault(require("unzipper")); // Add unzipper import
const execute_tests_2 = require("./execute-tests");
const swaggerDocument = require('../swagger.json');
const app = (0, express_1.default)();
const port = 3000;
app.use('/swagger', swagger_ui_express_1.default.serve, swagger_ui_express_1.default.setup(swaggerDocument));
app.use((0, cors_1.default)());
app.use(body_parser_1.default.json());
app.get('/', (req, res) => {
    res.send('Hello, Express!');
});
app.post('/api/execute/:exerciseName', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    const exerciseName = req.params.exerciseName;
    const fileName = req.body.fileName;
    const code = req.body.code;
    const templateFilePath = `./templates/${exerciseName}`;
    console.log(fileName);
    console.log(code);
    const result = yield (0, execute_tests_1.runTs)(exerciseName, templateFilePath, code, fileName);
    res.status(200).json(result);
}));
const storage = multer_1.default.diskStorage({
    destination: function (req, file, cb) {
        cb(null, './templates'); // Save uploaded files to the "uploads" directory
    },
    filename: function (req, file, cb) {
        cb(null, file.originalname);
    }
});
const upload = (0, multer_1.default)({ storage: storage });
// Upload route with zip file extraction
app.post('/uploadFullTemplate', upload.single('file'), (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    // Check if a file was uploaded
    if (!req.file) {
        return res.status(400).json({ error: 'No file uploaded' });
    }
    // Extract the uploaded zip file
    const zipFilePath = path_1.default.join(__dirname, '../templates', req.file.filename);
    yield unZip(zipFilePath);
    console.log("after unzipping ...");
    const fileNameSplitted = req.file.filename.split('.');
    console.log(fileNameSplitted[0]);
    let result = "...";
    console.log(req.body.content);
    if (req.body.content === "full") {
        result = yield (0, execute_tests_2.runTemplate)(`templates/${fileNameSplitted[0]}`);
        console.log(result);
    }
    else {
        result = "empty template uploaded";
    }
    console.log("before the end ...");
    // Remove the unzipped directory
    if (req.body.content === "full") {
        const unzippedDirPath = path_1.default.join(__dirname, '../templates', fileNameSplitted[0]);
        yield deleteFolderRecursive(unzippedDirPath);
    }
    fs_1.default.unlinkSync(zipFilePath);
    res.status(200).json(result);
}));
function deleteFolderRecursive(path) {
    return __awaiter(this, void 0, void 0, function* () {
        if (fs_1.default.existsSync(path)) {
            fs_1.default.readdirSync(path).forEach((file, index) => {
                const curPath = path + "/" + file;
                if (fs_1.default.lstatSync(curPath).isDirectory()) { // recurse
                    deleteFolderRecursive(curPath);
                }
                else { // delete file
                    fs_1.default.unlinkSync(curPath);
                }
            });
            fs_1.default.rmdirSync(path);
        }
    });
}
function unZip(zipFilePath) {
    return __awaiter(this, void 0, void 0, function* () {
        return new Promise((resolve, reject) => {
            fs_1.default.createReadStream(zipFilePath)
                .pipe(unzipper_1.default.Extract({ path: `/usr/src/app/templates` }))
                .on('close', () => {
                console.log('Zip file extracted successfully');
                resolve();
            })
                .on('error', (err) => {
                console.error('Error extracting zip file:', err);
                reject(err);
            });
        });
    });
}
app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});
