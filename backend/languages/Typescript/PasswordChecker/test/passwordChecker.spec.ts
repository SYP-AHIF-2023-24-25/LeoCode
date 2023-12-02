// passwordValidator.spec.ts
import { expect } from 'chai';
import { CheckPassword } from '../src/passwordChecker';

describe('CheckPassword Function', () => {
    it('should return true for a valid password', () => {
        const isValid = CheckPassword('validPwd');
        expect(isValid).to.equal(true);
    });

    it('should return false for an invalid password (too short)', () => {
        const isValid = CheckPassword('short');
        expect(isValid).to.equal(false);
    });

    it('should return false for an invalid password (too long)', () => {
        const isValid = CheckPassword('thisPasswordIsTooLong');
        expect(isValid).to.equal(false);
    });
});
