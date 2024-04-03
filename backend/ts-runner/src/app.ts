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
import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
import { runTs } from './execute-tests';
import fs from 'fs';
import swaggerUi from 'swagger-ui-express';
import { Snippets } from './model/snippets';
import path from 'path';
import { promisify } from 'util';
import { exec } from 'child_process';
import multer from 'multer';
import unzipper from 'unzipper'; // Add unzipper import

const swaggerDocument = require('../swagger.json');

const app = express();
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
  const result = await runTs(exerciseName, templateFilePath, code, fileName);
  res.status(200).json(result);
});

const storage = multer.diskStorage({
  destination: function (req: any, file: any, cb: any) {
    cb(null, './testExercises'); // Save uploaded files to the "uploads" directory
  },
  filename: function (req: any, file: any, cb: any) {
    cb(null, file.originalname);
  }
});

const upload = multer({ storage: storage });

// Upload route with zip file extraction
app.post('/upload', upload.single('file'), (req, res) => {
  // Check if a file was uploaded
  console.log("in the method");
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }
  console.log("file is there");
  // Extract the uploaded zip file
  /*const zipFilePath = path.join(__dirname, '../testExercises', req.file.filename);
  console.log(zipFilePath);
  fs.createReadStream(zipFilePath)
    .pipe(unzipper.Extract({ path: path.join(__dirname, './testExercises') }))
    .on('close', () => {
      res.json({ message: 'Zip file extracted successfully' });
    })
    .on('error', (err) => {
      console.error('Error extracting zip file:', err);
      res.status(500).json({ error: 'Error extracting zip file' });
    });*/
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});
