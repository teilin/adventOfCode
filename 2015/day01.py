current_floor = 0
position = 0
position_to_basement = 0
is_first = False

with open('day01.txt', 'r') as f:
    while 1:
        char = f.read(1)
        if not char:
            break
        position += 1
        if char == '(':
            current_floor += 1
        if char == ')':
            current_floor -= 1
        if current_floor < 0 and is_first == False:
            position_to_basement = position
            is_first = True

# Part 1
print(current_floor)

# Part 2
print(position_to_basement)