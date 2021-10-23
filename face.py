
import face_recognition
from PIL import Image, ImageDraw
import numpy as np

known_face_names = ["abed","ayman","basel","basem","qusai"]
known_face_encodings=[]
for name in known_face_names:
    image = face_recognition.load_image_file("images/"+name+".jpg")
    encoding = face_recognition.face_encodings(image)[0]
    known_face_encodings.append(encoding)
unknown_image = face_recognition.load_image_file("images/unknown.jpg")
face_locations = face_recognition.face_locations(unknown_image)
face_encodings = face_recognition.face_encodings(unknown_image, face_locations)
pil_image = Image.fromarray(unknown_image)
draw = ImageDraw.Draw(pil_image)
for (top, right, bottom, left), face_encoding in zip(face_locations, face_encodings):
    matches = face_recognition.compare_faces(known_face_encodings, face_encoding)
    name = "Unknown"
    face_distances = face_recognition.face_distance(known_face_encodings, face_encoding)
    best_match_index = np.argmin(face_distances)
    if matches[best_match_index]:
    name = known_face_names[best_match_index]
    draw.rectangle(((left, top), (right, bottom)), outline=(0, 0, 255))
    text_width, text_height = draw.textsize(name)
    draw.rectangle(((left, bottom - text_height - 10), (right, bottom)), fill=(0, 0, 255), outline=(0, 0, 255))
    draw.text((left + 6, bottom - text_height - 5), name, fill=(255, 255, 255, 255))
pil_image.show()
