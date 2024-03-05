import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
import { runCSharp } from './execute-tests';


const app = express();
const port = 3000;

app.use(cors());

app.use(bodyParser.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, Express!');
});

app.post('/api/execute/:exerciseName', async (req: Request, res: Response) => {
  const exerciseName = req.params.exerciseName;
  const code = req.body.code;
  const fileName = req.params.fileName;
  const templateFilePath = `./templates/${exerciseName}`;
  const result = await runCSharp(exerciseName, templateFilePath,code, fileName);
  res.status(200).json(result);
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});