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


app.post('/api/testTemplate', async (req: Request, res: Response) => {
  console.log("drinnen");
  const file: File = req.body.file;
  console.log(file);
  console.log(File.name);
  res.status(200).send('Test template received.');
});

app.post('/api/process-zip', async (req: Request, res: Response) => {
  console.log("Processing ZIP file");
  try {
      const zipData = req.body;

      const zipFilePath = path.join(__dirname, 'uploaded.zip');
      fs.writeFileSync(zipFilePath, zipData);

      const extractPath = path.join(__dirname, 'extracted');
      await promisify(require('extract-zip'))(zipFilePath, { dir: extractPath });

      exec('npm test', { cwd: extractPath }, (error, stdout, stderr) => {
          if (error) {
              console.error('Error running npm test:', error);
              res.status(500).send('Error running npm test.');
          } else {
              console.log('npm test output:', stdout);
              res.status(200).send('npm test completed successfully.');
          }
      });
  } catch (error) {
      console.error('Error processing ZIP file:', error);
      res.status(500).send('Internal server error.');
  }
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});