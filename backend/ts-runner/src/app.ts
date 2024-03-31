import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
import { runTs } from './execute-tests';

import swaggerUi from 'swagger-ui-express';
import { Snippets } from './model/snippets';

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
  let result = await runTs(exerciseName, templateFilePath,code,fileName);
  res.status(200).json(result);
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});