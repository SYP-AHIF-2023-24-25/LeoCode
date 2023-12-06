"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CheckPassword = void 0;
function CheckPassword(password) {
    if (password.length >= 6 && password.length <= 12) {
        return true;
    }
    return false;
}
exports.CheckPassword = CheckPassword;
console.log(CheckPassword("123456"));
