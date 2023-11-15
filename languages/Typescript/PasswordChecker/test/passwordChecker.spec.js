"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// passwordValidator.spec.ts
const chai_1 = require("chai");
const passwordChecker_1 = require("../src/passwordChecker");
describe('CheckPassword Function', () => {
    it('should return true for a valid password', () => {
        const isValid = (0, passwordChecker_1.CheckPassword)('validPwd');
        (0, chai_1.expect)(isValid).to.equal(true);
    });
    it('should return false for an invalid password (too short)', () => {
        const isValid = (0, passwordChecker_1.CheckPassword)('short');
        (0, chai_1.expect)(isValid).to.equal(false);
    });
    it('should return false for an invalid password (too long)', () => {
        const isValid = (0, passwordChecker_1.CheckPassword)('thisPasswordIsTooLong');
        (0, chai_1.expect)(isValid).to.equal(false);
    });
});
