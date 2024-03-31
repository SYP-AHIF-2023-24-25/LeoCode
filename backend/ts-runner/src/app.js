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
const swagger_ui_express_1 = __importDefault(require("swagger-ui-express"));
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
    let result = yield (0, execute_tests_1.runTs)(exerciseName, templateFilePath, code, fileName);
    res.status(200).json(result);
}));
app.listen(port, () => {
    console.log(`Server is running at http://localhost:${port}`);
});
