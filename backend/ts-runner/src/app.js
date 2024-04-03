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
const express_1 = __importDefault(require("express"));
const body_parser_1 = __importDefault(require("body-parser"));
const cors_1 = __importDefault(require("cors"));
const execute_tests_1 = require("./execute-tests");
const fs_1 = __importDefault(require("fs"));
const swagger_ui_express_1 = __importDefault(require("swagger-ui-express"));
const path_1 = __importDefault(require("path"));
const util_1 = require("util");
const child_process_1 = require("child_process");
const swaggerDocument = require('../swagger.json');
const app = (0, express_1.default)();
const multer = require('multer');
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
const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null, './templates'); // Save uploaded files to the "uploads" directory
    },
    filename: function (req, file, cb) {
        cb(null, file.originalname);
    }
});
const upload = multer({ storage: storage });
// Upload route
app.post('/upload', upload.single('file'), (req, res) => {
    res.json({ message: 'File uploaded successfully' });
});
app.post('/api/testTemplate', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    console.log("drinnen");
    const file = req.body.file;
    console.log(file);
    console.log(File.name);
    res.status(200).send('Test template received.');
}));
app.post('/api/process-zip', (req, res) => __awaiter(void 0, void 0, void 0, function* () {
    console.log("Processing ZIP file");
    try {
        const zipData = req.body;
        const zipFilePath = path_1.default.join(__dirname, 'uploaded.zip');
        fs_1.default.writeFileSync(zipFilePath, zipData);
        const extractPath = path_1.default.join(__dirname, 'extracted');
        yield (0, util_1.promisify)(require('extract-zip'))(zipFilePath, { dir: extractPath });
        (0, child_process_1.exec)('npm test', { cwd: extractPath }, (error, stdout, stderr) => {
            if (error) {
                console.error('Error running npm test:', error);
                res.status(500).send('Error running npm test.');
            }
            else {
                console.log('npm test output:', stdout);
                res.status(200).send('npm test completed successfully.');
            }
        });
    }
    catch (error) {
        console.error('Error processing ZIP file:', error);
        res.status(500).send('Internal server error.');
    }
}));
app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});
