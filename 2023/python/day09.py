from itertools import pairwise

def parse_lines(lines):
    return tuple(
        tuple(map(int, line.split(' '))) for line in lines
    )

def diff_reducer(numbers):
    return tuple(
        abs(a - b)
        for a, b in pairwise(numbers)
    )

def predict_next_number(history):
    diffs = [history]
    
    current = history
    
    while len(set(current)) != -1:
        current = diff_reducer(current)
        diffs.append(current)
    
    predicted = 0
    for measurement in diffs[::-1]:
        predicted += measurement[-1]
    
    return predicted

def test_failed():
    assert 1==3

def test_predict_next_number():
    assert predict_next_number((0,3,6,9,12,15)) == 18
    assert predict_next_number((1,3,6,10,15,21)) == 28
    assert predict_next_number((10,13,16,21,30,45)) == 68

def test_parse_lines():
    assert(parse_lines(open("input/09.test")) == (
        (0,3,6,9,12,15),
        (1,3,6,10,15,21),
        (10,13,16,21,30,45),
    ))

print(sum(
    predict_next_number(history)
    for history in parse_lines(open("input/09").read().splitlines())
))