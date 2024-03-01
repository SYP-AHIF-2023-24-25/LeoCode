import express, { Request, Response } from 'express';
import bodyParser from 'body-parser';
import cors from 'cors';
import { runTs, replaceCode } from './execute-tests';
import swaggerUi from 'swagger-ui-express';

const swaggerDocument = require('../swagger.json');

const app = express();
const port = 3000;

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(swaggerDocument));
app.use(cors());

app.use(bodyParser.json());

app.get('/', (req: Request, res: Response) => {
  res.send('Hello, Express!');
});

app.post('/api/execute/:exerciseId', async (req: Request, res: Response) => {
  const code = req.body.code;
  const exerciseId = req.params.exerciseId;
  await replaceCode(code,exerciseId);
  const templateFilePath = `/usr/src/app/templates/${exerciseId}`;
  console.log(templateFilePath);
  const result = await runTs(exerciseId, templateFilePath);
  res.status(200).json(result);
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
});

