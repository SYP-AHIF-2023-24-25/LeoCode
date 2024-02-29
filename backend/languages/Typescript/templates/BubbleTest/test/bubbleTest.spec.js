"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const chai_1 = require("chai");
const bubbleTest_1 = require("../src/bubbleTest");
describe('bubbleTest Function', () => {
    it('should return true for a bubble', () => {
        const isValid = (0, bubbleTest_1.BubbleTest)('bubble');
        (0, chai_1.expect)(isValid).to.equal(true);
    });
});
