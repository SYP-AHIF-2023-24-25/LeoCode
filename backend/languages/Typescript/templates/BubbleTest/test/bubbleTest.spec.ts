import { expect } from 'chai';
import { BubbleTest } from '../src/bubbleTest';

describe('bubbleTest Function', () => {
    it('should return true for a bubble', () => {
        const isValid = BubbleTest('bubble');
        expect(isValid).to.equal(true);
    });
});